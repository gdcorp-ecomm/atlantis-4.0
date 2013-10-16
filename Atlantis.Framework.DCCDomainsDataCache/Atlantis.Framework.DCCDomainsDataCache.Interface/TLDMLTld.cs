using System;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TLDMLTld : TLDMLNamespaceElement, ITLDTld  
  {
    protected override string Namespace
    {
      get { return "urn:godaddy:ns:tld"; }
    }

    protected override string LocalName
    {
      get { return "tld"; }
    }

    public TLDMLTld(XDocument tldmlDoc) : base(tldmlDoc)
    {
      _renewProhibitedPeriodForExpiration = LoadRenewProhibitedPeriodForExpiration();
      _isGtld = LoadIsGTld();
    }

    private readonly int _renewProhibitedPeriodForExpiration;
    public int RenewProhibitedPeriodForExpiration
    {
      get { return _renewProhibitedPeriodForExpiration; }
    }

    private string _renewProhibitedPeriodForExpirationUnit;
    public string RenewProhibitedPeriodForExpirationUnit
    {
      get { return _renewProhibitedPeriodForExpirationUnit; }
    }

    private bool _isGtld;
    public bool IsGtld
    {
      get { return _isGtld; }
    }

    private bool LoadIsGTld()
    {
      var gtldFalg = false;
      var isGTldAttribute = NamespaceElement.Attribute("isGTLD");
      if (isGTldAttribute != null)
      {
        gtldFalg = Convert.ToBoolean(isGTldAttribute.Value);
      }
      return gtldFalg;
    }

    private int LoadRenewProhibitedPeriodForExpiration()
    {
      int period = 0;
      _renewProhibitedPeriodForExpirationUnit = string.Empty;

      XElement collection = NamespaceElement.Descendants("renewprohibitedperiodcollection").FirstOrDefault();
      if (collection != null)
      {
        var renewPeriods = collection.Descendants("renewprohibitedperiod");
        foreach (var element in renewPeriods)
        {
          XAttribute refAtt = element.Attribute("reference");
          if (refAtt != null && refAtt.Value.ToLowerInvariant() == "expiration")
          {
            XAttribute unitAttribute = element.Attribute("unit");
            if (unitAttribute != null)
            {
              _renewProhibitedPeriodForExpirationUnit = unitAttribute.Value;
              switch (unitAttribute.Value)
              {
                case "month":
                  period = element.PeriodMonths();
                  break;
                case "day":
                  period = element.PeriodDays();
                  break;
                default:
                  _renewProhibitedPeriodForExpirationUnit = string.Empty;
                  break;
              }
            }
          }
        }
      }

      return period;
    }
  }
}
