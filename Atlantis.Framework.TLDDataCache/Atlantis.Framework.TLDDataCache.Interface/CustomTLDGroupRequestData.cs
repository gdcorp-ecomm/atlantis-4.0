using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class CustomTLDGroupRequestData : RequestData
  {
    private string _groupName;

    public CustomTLDGroupRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string groupName)
      :base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _groupName = groupName.ToUpperInvariant();
    }

    public string GroupName
    {
      get { return _groupName; }
    }

    public override string GetCacheMD5()
    {
      return _groupName;
    }

    public override string ToXML()
    {
      XElement element = new XElement("CustomTLDGroupRequestData");
      element.Add(new XAttribute("groupname", _groupName));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
