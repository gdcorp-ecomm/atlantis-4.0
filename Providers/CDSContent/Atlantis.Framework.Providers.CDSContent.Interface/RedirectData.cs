using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
