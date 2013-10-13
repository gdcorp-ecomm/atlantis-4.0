using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Newtonsoft.Json;

namespace Atlantis.Framework.Providers.CDSContent.Interface
{
  public class RedirectData : IRedirectData, IRenderContent
  {
    public RedirectData(string type, string location)
    {
      Type = type;
      Location = location;
    }

    #region IRedirectData Members

    [JsonProperty("Type")]
    public string Type { get; set; }

    [JsonProperty("Loc")]
    public string Location { get; set; }

    #endregion

    #region IRenderContent Members

    public string Content
    {
      get { return Location; }
    }

    #endregion
  }
}
