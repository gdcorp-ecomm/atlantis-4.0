using System;
using System.Globalization;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Support.Interface
{
  public class SupportPhoneRequestData : RequestData
  {
    public int ResellerTypeId { get; set; }
    public string CountryCode { get; set; }

    public SupportPhoneRequestData(int resellerTypeId, string countryCode)
    {
      ResellerTypeId = resellerTypeId;
      CountryCode = countryCode;

      RequestTimeout = TimeSpan.FromSeconds(3);
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(ResellerTypeId.ToString(CultureInfo.InvariantCulture), CountryCode);
    }

    public override string ToXML()
    {
      XElement element = new XElement("SupportSettingsRequestData");
      element.Add(new XAttribute("id", ResellerTypeId));
      element.Add(new XAttribute("countryCode", CountryCode));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
