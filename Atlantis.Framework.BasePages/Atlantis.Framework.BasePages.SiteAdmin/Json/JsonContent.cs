using System.Runtime.Serialization;

namespace Atlantis.Framework.BasePages.SiteAdmin.Json
{
  [DataContract()]
  public class JsonContent
  {
    public JsonContent(string targetDivID, string html, object data)
    {
      Html = html;
      TargetDivID = targetDivID;
      Data = data;
    }

    [DataMember()]
    public string Html { get; private set; }

    [DataMember()]
    public string TargetDivID { get; private set; }

    [DataMember()]
    public object Data { get; set; }
  }
}
