using Newtonsoft.Json;
namespace Atlantis.Framework.Language.Impl.Data
{
  internal class CDSContentVersion
  {
    public ContentId _id { get; set; }
    public ContentId DocumentId { get; set; }
    public string Content { get; set; }
  }

  public class ContentId
  {
    [JsonProperty("$oid")]
    public string oid { get; set; }
  }
}   
