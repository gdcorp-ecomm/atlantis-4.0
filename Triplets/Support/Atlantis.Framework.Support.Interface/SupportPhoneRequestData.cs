using System;
using System.Globalization;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Support.Interface
{
  public class SupportPhoneRequestData : RequestData
  {
    public int ResellerTypeId { get; set; }

    public SupportPhoneRequestData(int resellerTypeId)
    {
      ResellerTypeId = resellerTypeId;

      RequestTimeout = TimeSpan.FromSeconds(3);
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(ResellerTypeId.ToString(CultureInfo.InvariantCulture));
    }

    public override string ToXML()
    {
      var element = new XElement("SupportPhoneRequestData");
      element.Add(new XAttribute("ResellerTypeId", ResellerTypeId));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
