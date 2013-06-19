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

    private TldValidYearsSet _offeredRegistrationYears;
    private TldValidYearsSet _offeredTransferYears;
    private TldValidYearsSet _offeredRenewalYears;
    private TldValidYearsSet _offeredExpiredAuctionYears;
    private Dictionary<string, TldValidYearsSet> _offeredPreregistrationYears;
    private Dictionary<string, bool> _offeredPreRegApplicationFees = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

    public TLDMLProduct(XDocument tldmlDoc)
      : base(tldmlDoc)
    {
      _offeredRegistrationYears = LoadValidYears("registrationperiodcollection", "registrationperiod");
      _offeredTransferYears = LoadValidYears("transferperiodcollection", "transferperiod");
      _offeredRenewalYears = LoadValidYears("renewperiodcollection", "renewperiod");
      _offeredExpiredAuctionYears = LoadValidYears("expiredauctionperiodcollection", "expiredauctionperiod");
      _offeredPreregistrationYears = LoadPreregistrationYears();
    }

    private TldValidYearsSet LoadValidYears(string periodCollectionName, string periodItemName)
    {
      TldValidYearsSet result;

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

    private Dictionary<string, TldValidYearsSet> LoadPreregistrationYears()
    {
      var phaseYears = new Dictionary<string, TldValidYearsSet>(StringComparer.OrdinalIgnoreCase);

      var launchRegCollections = NamespaceElement.Descendants("launchregistrationcollection");
      foreach (XElement launchRegCollection in launchRegCollections)
      {
        foreach (var phase in launchRegCollection.Descendants("launchregistration"))
        {
          XAttribute valueAtt = phase.Attribute("code");
          if (valueAtt != null)
          {
            phaseYears[valueAtt.Value] = TldValidYearsSet.FromPeriodElements(phase.Descendants("launchregistrationperiod"));

            //load the application fee dictionary for each phase
            LoadPhasesApplicationFees(phase, valueAtt);
          }
        }
      }

      return phaseYears;
    }

    private void LoadPhasesApplicationFees(XElement phase, XAttribute valueAtt)
    {
      var applicationElement = phase.Descendants("applicationFee").FirstOrDefault();
      if (applicationElement != null)
      {
        _offeredPreRegApplicationFees[valueAtt.Value] = "true".Equals(applicationElement.Attribute("enabled").Value,
                                                              StringComparison.OrdinalIgnoreCase);
      }
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
      TldValidYearsSet result;
      if (!_offeredPreregistrationYears.TryGetValue(type, out result))
      {
        result = TldValidYearsSet.INVALIDSET;
      }
      return result;
    }

    public bool HasPreRegApplicationFee(string type)
    {
      bool result;
      _offeredPreRegApplicationFees.TryGetValue(type, out result);

      return result;
    }
  }
}
