using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Newtonsoft.Json;

namespace Atlantis.Framework.Providers.CDSContent.Interface
{
  public class RouteData : IRenderContent
  {
    public RouteData() { }

    public RouteData(string loc)
    {
      Location = loc;
    }

    [JsonProperty("Loc")]
    public string Location { get; set; }

    #region IRenderContent Members

    public string Content
    {
      get { return Location; }
    }

    #endregion
  }
}
