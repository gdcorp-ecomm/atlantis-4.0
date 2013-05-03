using Atlantis.Framework.Render.Pipeline.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantis.Framework.CDS.Interface
{
  internal class ContentVersion
  {
    public string Content { get; set; }
    public string ContentType { get; set; }
    public string Name { get; set; }
    public ContentId _id { get; set; }
    public ContentId DocumentId { get; set; }
    public string Url { get; set; }
    public bool Status { get; set; }
    public ContentDate ActiveDate { get; set; }
    public ContentDate PublishDate { get; set; }
    public ContentUser User { get; set; } 
  }

  public class ContentId
  {
    [JsonProperty("$oid")]
    public string oid { get; set; }
  }

  public class ContentUser
  {
    [JsonProperty("FullName")]
    public string FullName { get; set; }
  }

  public class ContentDate
  {
    private string aDate;

    [JsonProperty("$date")]
    public string ADate
    {
      get { return aDate; }
      set
      {
        long ms;
        aDate = long.TryParse(value, out ms) ? new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(ms).ToString("F") : null;
      }
    }
  }
}
