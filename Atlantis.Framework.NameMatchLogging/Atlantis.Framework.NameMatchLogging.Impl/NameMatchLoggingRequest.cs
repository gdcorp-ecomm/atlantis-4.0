using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.NameMatchLogging.Interface;
using System.ServiceModel;
using System.Xml;
using System.Xml.Linq;

namespace Atlantis.Framework.NameMatchLogging.Impl
{
  public class NameMatchLoggingRequest : IRequest
  {
    #region IRequestMembers

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;

      try
      {
        NameMatchLoggingRequestData oNameMatchLoggingRequestData = (NameMatchLoggingRequestData)oRequestData;
        WsConfigElement wsConfig = ((WsConfigElement)oConfig);

        NameMatchLogging.DomainTokenizationClient client = GetWebServiceInstance(wsConfig.WSURL, oNameMatchLoggingRequestData.RequestTimeout);
        client.LogDomainsBotSpunData(GetRequestXML(oNameMatchLoggingRequestData));

        oResponseData = new NameMatchLoggingResponseData(false);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new NameMatchLoggingResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new NameMatchLoggingResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    public string GetRequestXML(NameMatchLoggingRequestData rqdata)
    {            
      XElement anchorServiceData = new XElement("DomainsBotData"
                                    , new XElement("visitGuid", rqdata.Pathway)
                                    , new XElement("shopperID", rqdata.ShopperID
                                                              , new XAttribute("shopperStatus", rqdata.ShopperStatus))
                                    , new XElement("sequence", rqdata.PageCount)
                                    , new XElement("searchedDomainName"
                                                    , rqdata.SearchedDomain
                                                    , new XAttribute("SLD", rqdata.Sld)
                                                    , new XAttribute("TLD", rqdata.Tld))
                                    , new XElement("promoTrackingCode", rqdata.PromoTrackingCode));

      XElement suggestionData = new XElement("suggestedDomains"
                                    , from s in rqdata.SuggestedDomains
                                      select new XElement("suggestedDomainName"
                                                          , s.DomainName
                                                          , new XAttribute("order", s.Order)
                                                          , new XAttribute("SLD", s.Sld)
                                                          , new XAttribute("TLD", s.Tld)));

      anchorServiceData.Add(suggestionData);
      return anchorServiceData.ToString();
      
    }

    #endregion

    private NameMatchLogging.DomainTokenizationClient GetWebServiceInstance(string webServiceUrl, TimeSpan requestTimeout)
    {
      BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
      basicHttpBinding.SendTimeout = requestTimeout;
      basicHttpBinding.OpenTimeout = requestTimeout;
      basicHttpBinding.CloseTimeout = requestTimeout;

      EndpointAddressBuilder endpointAddressBuilder = new EndpointAddressBuilder();
      endpointAddressBuilder.Identity = EndpointIdentity.CreateDnsIdentity("localhost");
      endpointAddressBuilder.Uri = new Uri(webServiceUrl);

      return new NameMatchLogging.DomainTokenizationClient(basicHttpBinding, endpointAddressBuilder.ToEndpointAddress());
    }
  }
}
