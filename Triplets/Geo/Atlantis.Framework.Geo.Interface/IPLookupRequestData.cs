using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Interface
{
  /// <summary>
  /// This base class exists so it can be used in the future for other IP based Geo lookups:
  /// Like IPv6 and more specific location information
  /// </summary>
  public abstract class IPLookupRequestData : RequestData
  {
    public string IpAddress { get; private set; }

    public IPLookupRequestData(string ipAddress)
    {
      IpAddress = ipAddress;
    }

    public override string ToXML()
    {
      XElement element = new XElement(this.GetType().Name);
      element.Add(new XAttribute("IP", IpAddress));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
