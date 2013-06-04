using Atlantis.Framework.Render.Pipeline.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
