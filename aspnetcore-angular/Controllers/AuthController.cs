using AspnetCoreAngular.Auth;
using AspnetCoreAngular.Helpers;
using AspnetCoreAngular.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspnetCoreAngular.Controllers
{
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    public AuthController(IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
      : base(jwtFactory, jwtOptions)
    {
    }

    // POST api/auth/login
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Post([FromBody]UserCredential credentials)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
      if (identity == null)
      {
        return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
      }

      // Serialize and return the response
      var response = new
      {
        id = identity.Claims.Single(c => c.Type == "id").Value,
        auth_token = await _jwtFactory.GenerateEncodedToken(credentials.UserName, identity),
        expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
      };

      var json = JsonConvert.SerializeObject(response, _serializerSettings);
      return new OkObjectResult(json);
    }

    private Task GetClaimsIdentity(object userName, object password)
    {
      throw new NotImplementedException();
    }

    private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
    {
      if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
      {
        if (userName.Equals("123") && password.Equals("123"))
        {
          var user = new UserCredential { Id = Guid.NewGuid().ToString(), UserName = userName, Password = password };

          return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, user.Id));
        }
      }

      // Credentials are invalid, or account doesn't exist
      return await Task.FromResult<ClaimsIdentity>(null);
    }
  }
}
