using Atlantis.Framework.TLDDataCache.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public static class TLDRenewal
  {
    public static int? GetRenewalMonthsBeforeExpiration(string tld)
    {
      int? result = GetRenewalMonthsBeforeExpirationInternal(tld);

      return result;
    }

    private static int? GetRenewalMonthsBeforeExpirationInternal(string tld)
    {
      int? renewalMonthsBeforeExpiration = null;

      var extendedTLDData = GetExtendedTLDData(tld);
      if (extendedTLDData != null)
      {
        string tldDataXML = null;
        if (extendedTLDData.TryGetValue("tldData", out tldDataXML))
        {
          XElement tldDataElement = null;
          try
          {
            tldDataElement = XElement.Parse(tldDataXML);
          }
          catch
          {
            //do nothing (assume that there is no max expiration months elig. data)
          }

          if (tldDataElement != null)
          {
            IEnumerable<string> maxExpirationMonths = from x
                                                in tldDataElement.Elements("RENEWALS")
                                                      where x.Attributes("maxExpirationMonthsElig").Any()
                                                      select x.Attribute("maxExpirationMonthsElig").Value;

            if (maxExpirationMonths.Count() == 1)
            {
              string monthString = maxExpirationMonths.Single();
              int month;
              if (int.TryParse(monthString, out month))
              {
                renewalMonthsBeforeExpiration = month;
              }
            }
          }
        }
      }

      return renewalMonthsBeforeExpiration;
    }

    private static ExtendedTLDDataResponseData GetExtendedTLDData(string tld)
    {
      var request = new ExtendedTLDDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, tld);
      return (ExtendedTLDDataResponseData)DataCache.DataCache.GetProcessRequest(request, StaticDotTypeEngineRequests.ExtendedTLDData);
    }

  }
}