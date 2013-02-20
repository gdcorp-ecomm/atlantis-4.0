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
      get { return "urn:godaddy:ns:product-1.0"; }
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

    public TLDMLProduct(XDocument tldmlDoc)
      : base(tldmlDoc)
    {
      _offeredRegistrationYears = LoadValidYears("registrationperiodcollection", "registrationperiod");
      _offeredTransferYears = LoadValidYears("transferperiodcollection", "transferperiod");
      _offeredRenewalYears = LoadValidYears("renewperiodcollection", "renewperiod");
      _offeredExpiredAuctionYears = LoadValidYears("expiredauctionperiodcollection", "expiredauctionperiod");
      _offeredPreregistrationYears = LoadPreregistrationYears();
    }

    private Dictionary<string, TldValidYearsSet> LoadPreregistrationYears()
    {
      Dictionary<string, TldValidYearsSet> result = new Dictionary<string, TldValidYearsSet>(StringComparer.OrdinalIgnoreCase);

      var preregistrationCollections = NamespaceElement.Descendants("preregistrationperiodcollection");
      foreach (XElement preregCollection in preregistrationCollections)
      {
        XAttribute typeAtt = preregCollection.Attribute("type");
        if (typeAtt != null)
        {
          result[typeAtt.Value] = TldValidYearsSet.FromPeriodElements(preregCollection.Descendants("preregistrationperiod"));
        }
      }

      return result;
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

  }
}
