using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TLDMLProduct : TLDMLNamespaceElement, ITLDProduct  
  {

    protected override string Namespace
    {
      get { return "urn:godaddy:ns:product"; }
    }

    protected override string LocalName
    {
      get { return "product"; }
    }

    private ITLDValidYearsSet _offeredRegistrationYears;
    private ITLDValidYearsSet _offeredTransferYears;
    private ITLDValidYearsSet _offeredRenewalYears;
    private ITLDValidYearsSet _offeredExpiredAuctionYears;
    private Dictionary<string, ITLDValidYearsSet> _offeredPreregistrationYears;
    private Dictionary<string, bool> _offeredPhaseApplicationFees = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
    private Dictionary<string, string> _offeredPhaseApplicationProducts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    public TLDMLProduct(XDocument tldmlDoc)
      : base(tldmlDoc)
    {
      _offeredRegistrationYears = LoadValidYears("registrationperiodcollection", "registrationperiod");
      _offeredTransferYears = LoadValidYears("transferperiodcollection", "transferperiod");
      _offeredRenewalYears = LoadValidYears("renewperiodcollection", "renewperiod");
      _offeredExpiredAuctionYears = LoadValidYears("expiredauctionperiodcollection", "expiredauctionperiod");
      _offeredPreregistrationYears = LoadPreregistrationYears();

      LoadTrustee();
      LoadRegistryPremiumDomains();
    }


    private ITLDValidYearsSet LoadValidYears(string periodCollectionName, string periodItemName)
    {
      ITLDValidYearsSet result;

      XElement periodCollection = NamespaceElement.Descendants(periodCollectionName).FirstOrDefault();
      if (periodCollection != null)
      {
        result = TldValidYearsSet.FromPeriodElements(periodCollection.Descendants(periodItemName));
      }
      else
      {
        result = TldValidYearsSet.INVALIDSET;
      }

      return result;
    }

    private Dictionary<string, ITLDValidYearsSet> LoadPreregistrationYears()
    {
      var phaseYears = new Dictionary<string, ITLDValidYearsSet>(StringComparer.OrdinalIgnoreCase);

      var launchRegCollections = NamespaceElement.Descendants("launchregistrationcollection");
      foreach (XElement launchRegCollection in launchRegCollections)
      {
        foreach (var phase in launchRegCollection.Descendants("launchregistration"))
        {
          XAttribute valueAtt = phase.Attribute("code");
          if (valueAtt != null)
          {
            phaseYears[valueAtt.Value] = TldValidYearsSet.FromPeriodElements(phase.Descendants("launchregistrationperiod"));

            LoadPhasesApplicationFees(phase, valueAtt);
          }
        }
      }

      return phaseYears;
    }

    private void LoadPhasesApplicationFees(XElement phase, XAttribute valueAtt)
    {
      var applicationElement = phase.Descendants("applicationfee").FirstOrDefault();
      if (applicationElement != null)
      {
        var applicationElementEnabled = applicationElement.Attribute("enabled").Value;
        _offeredPhaseApplicationFees[valueAtt.Value] = "true".Equals(applicationElementEnabled, StringComparison.OrdinalIgnoreCase);
        
        if ("true".Equals(applicationElementEnabled, StringComparison.OrdinalIgnoreCase))
        {
          var productTypeAttr = applicationElement.Attribute("producttype");
          if (productTypeAttr != null)
          {
            _offeredPhaseApplicationProducts[valueAtt.Value] = applicationElement.Attribute("producttype").Value;
          }
        }
      }
    }

    private void LoadRegistryPremiumDomains()
    {
      RegistryPremiumDomains = TLDMLRegistryPremiumDomains.Create(NamespaceElement.Descendants("registrypremiumdomains"));
    }

    private void LoadTrustee()
    {
      Trustee = TLDMLTrustee.Create(NamespaceElement.Descendants("trustee"));
    }

    public ITLDValidYearsSet RegistrationYears
    {
      get { return _offeredRegistrationYears; }
    }

    public ITLDValidYearsSet TransferYears
    {
      get { return _offeredTransferYears; }
    }

    public ITLDValidYearsSet RenewalYears
    {
      get { return _offeredRenewalYears; }
    }

    public ITLDValidYearsSet ExpiredAuctionsYears
    {
      get { return _offeredExpiredAuctionYears; }
    }

    public ITLDValidYearsSet PreregistrationYears(string type)
    {
      ITLDValidYearsSet result;
      if (!_offeredPreregistrationYears.TryGetValue(type, out result))
      {
        result = TldValidYearsSet.INVALIDSET;
      }
      return result;
    }

    public bool HasPhaseApplicationFee(string phaseCode, out string applicationProductType)
    {
      applicationProductType = string.Empty;
      bool result;
      if (_offeredPhaseApplicationFees.TryGetValue(phaseCode, out result))
      {
        _offeredPhaseApplicationProducts.TryGetValue(phaseCode, out applicationProductType);
      }

      return result;
    }

    public List<int> GetPhaseApplicationProductIdList(string applicationProductType)
    {
      var result = new List<int>();

      var productTypeCollection = NamespaceElement.Descendants("producttype");
      foreach (var productType in productTypeCollection)
      {
        var valueAttr = productType.Attribute("value");
        if (valueAttr != null && valueAttr.Value.Equals(applicationProductType))
        {
          var pfCollection = productType.Descendants("pf");
          foreach (var pf in pfCollection)
          {
            if (pf.IsEnabled())
            {
              int nId;
              if (Int32.TryParse(pf.Attribute("id").Value, out nId))
              {
                result.Add(nId);
              }
            }
          }
          break;
        }
      }

      return result;
    }

    public ITLDTrustee  Trustee { get; private set; }

    public ITLDRegistryPremiumDomains RegistryPremiumDomains { get; private set; }
  }
}
