using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.DotTypeCache
{
  public class TLDRenewal
  {
    private static Dictionary<string, int?> _renewalMonthsBeforeExpiration;

    #region Constructor

    static TLDRenewal()
    {
      _renewalMonthsBeforeExpiration = new Dictionary<string, int?>();
    }
    #endregion

    #region methods

    public static int? GetRenewalMonthsBeforeExpiration(string tld)
    {
      int? result = null;

      if (!_renewalMonthsBeforeExpiration.TryGetValue(tld, out result))
      {
        result = GetRenewalMonthsBeforeExpirationInternal(tld);
        _renewalMonthsBeforeExpiration.Add(tld, result);
      }

      return result;
    }

    private static int? GetRenewalMonthsBeforeExpirationInternal(string tld)
    {
      int? renewalMonthsBeforeExpiration = null;

      var extendedTLDData = DataCache.DataCache.GetExtendedTLDData(tld);
      if (extendedTLDData != null && extendedTLDData.ContainsKey(tld))
      {
        var tldDictionary = extendedTLDData[tld];
        string tldDataXML = null;

        if (tldDictionary.TryGetValue("tldData", out tldDataXML))
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
    #endregion

  }
}