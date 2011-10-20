using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegVendorDomainSearch.Interface
{
  public class RegVendorDomainSearchResponseData : IResponseData
  {
    private AtlantisException _exception;
    private string _responseXml = string.Empty;

    public bool _isSuccess = false;
    public bool IsSuccess
    {
      get { return this._isSuccess; }
    }

    private RegVendorDomainSearchResult _domainSearchResult;

    public RegVendorDomainSearchResult DomainSearchResult
    {
      get { return _domainSearchResult; }
    }

    public RegVendorDomainSearchResponseData(string responseXML)
    {
      _responseXml = responseXML;
      PopulateFromXML(responseXML);
    }

    public RegVendorDomainSearchResponseData(string responseXML, AtlantisException exAtlantis)
    {
      _responseXml = responseXML;
      _exception = exAtlantis;
    }

    public RegVendorDomainSearchResponseData(string responseXML, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXML;
      _exception = new AtlantisException(oRequestData,
                                   "DPPDomainSearchResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

    /*
     Output Example:
     <domainsearch datetime="12/14/2010 7:58:09 AM" server="p3pwlep001" > 
       <exact>
         <domain vendorid="0" id="42376031" name="premiumcigarclub.com"  />
       </exact>
       <premiumlist>
         <domain vendorid="1" name="PremiumWeb.org" price="200" commissionpct="40"  />
         <domain vendorid="1" name="PremiumNet.net" price="250" commissionpct="40"  />
         <domain vendorid="2" name="PremiumFax.com" price="200" commissionpct="30"  />
         <domain vendorid="2" name="PremiumMed.com" price="300" commissionpct="30"  />
         <domain vendorid="3" name="PremiumCar.net" price="500"  />
        </premiumlist>
     </dppdomainsearch>
    */

    void PopulateFromXML(string resultXml)
    {
      XmlDocument xdDoc = new XmlDocument();
      xdDoc.LoadXml(resultXml);
      _domainSearchResult = new RegVendorDomainSearchResult();

      XmlElement xnDomainSearch = (XmlElement)xdDoc.SelectSingleNode("/dppdomainsearch");
      if (xnDomainSearch != null)
      {
        _isSuccess = true;

        foreach (XmlAttribute attr in xnDomainSearch.Attributes)
        {
          if (attr.Name == "date")
            _domainSearchResult.DateTime = xnDomainSearch.Attributes["date"].Value;
          else if (attr.Name == "server")
            _domainSearchResult.Server = xnDomainSearch.Attributes["server"].Value;
        }
      }
      else
      {
        _isSuccess = false;
        return;
      }

      // Exact domain match
      XmlElement xnExact = (XmlElement)xdDoc.SelectSingleNode("/dppdomainsearch/exact/domain");
      if (xnExact != null)
      {
        DPPDomainNameMatch match = new DPPDomainNameMatch();
        foreach (XmlAttribute attr in xnExact.Attributes)
        {
          handleAttribute(ref match, attr.Name, attr.Value);
        }

        _domainSearchResult.AddExactMatch(match);
      }

      // premium domain match
      XmlElement xnPremium = (XmlElement)xdDoc.SelectSingleNode("/dppdomainsearch/premiumlist");
      if (xnPremium != null)
      {
        foreach (XmlElement xnDomain in xnPremium)
        {
          DPPDomainNameMatch match = new DPPDomainNameMatch();
          foreach (XmlAttribute attr in xnDomain.Attributes)
          {
            handleAttribute(ref match, attr.Name, attr.Value);
          }
          _domainSearchResult.AddPremiumMatch(match);
        }
      }
    }

    private void handleAttribute(ref DPPDomainNameMatch match, string attribName, string attribValue)
    {
      if (attribName.CompareTo("vendorid") == 0)
      {
        int vendorId = (int)RegVendorDomainSearchVendor.None;
        int.TryParse(attribValue, out vendorId);
        match.VendorId = (RegVendorDomainSearchVendor)vendorId;
      }
      else if (attribName.CompareTo("id") == 0)
      {
        int id = 0;
        int.TryParse(attribValue, out id);
        match.AuctionId = id;
      }
      else if (attribName.CompareTo("name") == 0)
      {
        match.Name = attribValue;
      }
      else if (attribName.CompareTo("price") == 0)
      {
        int price = 0;
        int.TryParse(attribValue, out price);
        match.Price = price;
      }
      else if (attribName.CompareTo("commissionpct") == 0)
      {
        int pct = 0;
        int.TryParse(attribValue, out pct);
        match.CommissionPct = pct;
      }
      else if (attribName.CompareTo("auctiontype") == 0)
      {
        match.AuctionType = attribValue;
      }
      else if (attribName.CompareTo("auctionendtime") == 0)
      {
        DateTime endTime = DateTime.Now;
        DateTime.TryParse(attribValue, out endTime);
        match.AuctionEndTime = endTime;
      }

    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseXml;
    }

    #endregion
  }
}
