using AspnetCoreAngular.Auth;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace SharepointListControl
{
  public class Startup
  {
    public IConfigurationRoot Configuration { get; }

    private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
    private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

    public Startup(IHostingEnvironment env)
    {
      // https://joonasw.net/view/asp-net-core-1-configuration-deep-dive
      // .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddXmlFile("queries.xml", optional: true, reloadOnChange: true)
          .AddEnvironmentVariables();

      Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
      // http://www.c-sharpcorner.com/article/setting-and-reading-values-from-app-settings-json-in-net-core/
      // https://stackoverflow.com/questions/40874640/value-cannot-be-null-parameter-name-connectionstring-appsettings-json-in-start
      services.AddSingleton<IConfiguration>(Configuration);
      services.AddSingleton<IJwtFactory, JwtFactory>();

      // jwt wire up
      // Get options from app settings
      var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

      // Configure JwtIssuerOptions
      services.Configure<JwtIssuerOptions>(options =>
      {
        options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
        options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
        options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
      });

      // api user claim policy
      services.AddAuthorization(options =>
      {
        options.AddPolicy("ApiUser", policy =>
          policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
      });

      services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

      services.AddAutoMapper();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseExceptionHandler(
      builder =>
      {
        builder.Run(
                async context =>
                {
                  context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                  context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                  var error = context.Features.Get<IExceptionHandlerFeature>();
                  if (error != null)
                  {
                    //context.Response.AddApplicationError(error.Error.Message);
                    await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                  }
                });
      });

      var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

        ValidateAudience = true,
        ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = _signingKey,

        RequireExpirationTime = false,
        ValidateLifetime = false,
        ClockSkew = TimeSpan.Zero
      };

      app.UseJwtBearerAuthentication(new JwtBearerOptions
      {
        AutomaticAuthenticate = true,
        AutomaticChallenge = true,
        TokenValidationParameters = tokenValidationParameters
      });

      app.Use(async (context, next) =>
      {
        await next();
        if (context.Response.StatusCode == 404 &&
           !Path.HasExtension(context.Request.Path.Value) &&
           !context.Request.Path.Value.StartsWith("/api/"))
        {
          context.Request.Path = "/index.html";
          await next();
        }
      });
      app.UseMvcWithDefaultRoute();
      app.UseDefaultFiles();
      app.UseStaticFiles();
    }
  }
}
