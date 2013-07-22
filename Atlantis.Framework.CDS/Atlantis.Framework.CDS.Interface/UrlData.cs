using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Interface
{
  internal class UrlData : IUrlData
  {
    [JsonProperty("style")]
    public string Style { get; set; }
  }
}
