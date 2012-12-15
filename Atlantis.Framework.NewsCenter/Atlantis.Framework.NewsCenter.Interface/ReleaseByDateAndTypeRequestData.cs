using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.NewsCenter.Interface
{
  public class ReleaseByDateAndTypeRequestData : RequestData
  {
    public string DisplayDateKey {get; private set; }
    public string ReleaseType { get; private set; }

    public ReleaseByDateAndTypeRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, DateTime displayDate, string releaseType)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      DisplayDateKey = displayDate.Date.ToShortDateString();
      ReleaseType = releaseType;
    }

    public override string GetCacheMD5()
    {
      // because the key is shorter than a hash, just use the key
      return string.Concat(ReleaseType, "-", DisplayDateKey);
    }

    public override string ToXML()
    {
      XElement element = new XElement("ReleaseByDateAndTypeRequestData");
      element.Add(new XAttribute("displaydatekey", DisplayDateKey), new XAttribute("releasetype", ReleaseType));
      return element.ToString();
    }
  }
}
