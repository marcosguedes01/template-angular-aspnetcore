using AspnetCoreAngular.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AspnetCoreAngular.Controllers
{
  public class ControllerBase : Controller
  {
    protected readonly IJwtFactory _jwtFactory;
    protected readonly JsonSerializerSettings _serializerSettings;
    protected readonly JwtIssuerOptions _jwtOptions;

    public ControllerBase(IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
    {
      _jwtFactory = jwtFactory;
      _jwtOptions = jwtOptions.Value;

      _serializerSettings = new JsonSerializerSettings
      {
        Formatting = Formatting.Indented
      };
    }
  }
}
