using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.NewsCenter.Interface
{
  public class ReleaseByDateRequestData : RequestData
  {
    public string DisplayDateKey {get; private set; }

    public ReleaseByDateRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, DateTime displayDate)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      DisplayDateKey = displayDate.Date.ToShortDateString();
    }

    public override string GetCacheMD5()
    {
      // because the key is shorter than a hash, just use the key
      return DisplayDateKey;
    }

    public override string ToXML()
    {
      XElement element = new XElement("ReleaseByDateRequestData");
      element.Add(new XAttribute("displaydatekey", DisplayDateKey));
      return element.ToString();
    }
  }
}
