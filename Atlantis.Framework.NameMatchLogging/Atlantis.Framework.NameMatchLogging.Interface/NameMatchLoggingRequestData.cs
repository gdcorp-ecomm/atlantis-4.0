using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.NameMatchLogging.Interface
{
  public class NameMatchLoggingRequestData : RequestData
  {

    #region properties

    private string _searchedDomain;
    public string SearchedDomain
    {
      get { return _searchedDomain; }
      set { _searchedDomain = value; }
    }

    private string _promoTrackingCode;
    public string PromoTrackingCode
    {
      get { return _promoTrackingCode; }
      set { _promoTrackingCode = value; }
    }

    private List<SuggestedDomain> _suggestedDomains;
    public List<SuggestedDomain> SuggestedDomains
    {
      get { return _suggestedDomains; }
      set { _suggestedDomains = value; }
    }

    private string _sld;
    public string Sld
    {
      get { return _sld; }
      set { _sld = value; }
    }

    private string _tld;
    public string Tld
    {
      get { return _tld; }
      set { _tld = value; }
    }

    private string _shopperStatus;
    public string ShopperStatus
    {
      get { return _shopperStatus; }
      set { _shopperStatus = value; }
    }

    #endregion

    public NameMatchLoggingRequestData(string pathway
                                      , string shopperId
                                      , string shopperStatus
                                      , string sourceUrl
                                      , string orderId
                                      , int pageCount
                                      , string searchedDomain
                                      , string sld
                                      , string tld
                                      , string promoTrackingCode
                                      , List<SuggestedDomain> suggestedDomains
                                      , TimeSpan requestTimeout)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      SearchedDomain = searchedDomain;
      PromoTrackingCode = promoTrackingCode;
      SuggestedDomains = suggestedDomains;
      RequestTimeout = requestTimeout;
      ShopperStatus = shopperStatus;
      Sld = sld;
      Tld = tld;
    }


    public override string GetCacheMD5()
    {
      throw new Exception("NameMatchLogging is not a cacheable request.");
    }

    public string GetRequestXML()
    {
      XElement anchorServiceData = new XElement("DomainsBotData"
                                    , new XElement("visitGuid", this.Pathway)
                                    , new XElement("shopperID", this.ShopperID
                                                              , new XAttribute("shopperStatus", this.ShopperStatus))
                                    , new XElement("sequence", this.PageCount)
                                    , new XElement("searchedDomainName"
                                                    , this.SearchedDomain
                                                    , new XAttribute("SLD", this.Sld)
                                                    , new XAttribute("TLD", this.Tld))
                                    , new XElement("promoTrackingCode", this.PromoTrackingCode));

      XElement suggestionData = new XElement("suggestedDomains"
                                    , from s in this.SuggestedDomains
                                      select new XElement("suggestedDomainName"
                                                          , s.DomainName
                                                          , new XAttribute("order", s.Order)
                                                          , new XAttribute("SLD", s.Sld)
                                                          , new XAttribute("TLD", s.Tld)));

      anchorServiceData.Add(suggestionData);
      return anchorServiceData.ToString();

    }
  }
}
