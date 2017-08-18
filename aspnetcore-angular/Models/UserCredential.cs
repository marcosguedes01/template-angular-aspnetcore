using Newtonsoft.Json;

namespace AspnetCoreAngular.Models
{
  public class UserCredential
  {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("userName")]
    public string UserName { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }
  }
}
