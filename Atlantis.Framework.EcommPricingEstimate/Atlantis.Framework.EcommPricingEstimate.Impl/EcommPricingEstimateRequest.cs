using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.EcommPricingEstimate.Interface;
using Atlantis.Framework.EcommPricingEstimate.Impl;
using Atlantis.Framework.EcommPricingEstimate.Impl.WsgdPricingEstimate;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricingEstimate.Impl
{
  public class EcommPricingEstimateRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommPricingEstimateResponseData oResponseData = null;

      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement)config;
        string authServiceUrl = wsConfigElement.WSURL;
        using (Service authenticationService = new Service())
        {
          EcommPricingEstimateRequestData request = (EcommPricingEstimateRequestData)requestData;
          authenticationService.Url = authServiceUrl;
          authenticationService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          X509Certificate2 clientCertificate = wsConfigElement.GetClientCertificate();
          if (clientCertificate == null)
          {
            throw new Exception("Client certificate not found.");
          }
          authenticationService.ClientCertificates.Add(clientCertificate);
          string responseXml = authenticationService.GetPriceEstimateEx(request.ToXML());
          oResponseData = ParseResults(responseXml, requestData);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + " | " + ex.StackTrace;
        AtlantisException aex = new AtlantisException(requestData, "EcommPricingEstimateRequest.RequestHandler", message, string.Empty);
        oResponseData = new EcommPricingEstimateResponseData(aex);
      }
      return oResponseData;
    }

    private EcommPricingEstimateResponseData ParseResults(string resultXML, RequestData requestData)
    {
      EcommPricingEstimateResponseData responseData = new EcommPricingEstimateResponseData();
      XDocument resultData = XDocument.Parse(resultXML);
      XElement currentElement = (XElement)resultData.Root;
      if (currentElement != null)
      {
        responseData.MembershipLevel = int.Parse(currentElement.Attribute(PriceEstimateXmlNames.membershipLevel).Value);
        responseData.PrivateLabelID = int.Parse(currentElement.Attribute(PriceEstimateXmlNames.privateLabelID).Value);
        responseData.TransactionCurrency = currentElement.Attribute(PriceEstimateXmlNames.transactionCurrency).Value;

        foreach (XNode node in resultData.Root.Nodes())
        {
          try
          {
            XElement itemElement = (XElement)node;
            int pf_id = int.Parse(itemElement.Attribute(PriceEstimateXmlNames.pf_id).Value);
            string discount_code = itemElement.Attribute(PriceEstimateXmlNames.discount_code).Value;
            PriceEstimateItem pei = new PriceEstimateItem(pf_id, discount_code);
            pei.name = itemElement.Attribute(PriceEstimateXmlNames.name).Value;
            pei.list_price = int.Parse(itemElement.Attribute(PriceEstimateXmlNames.list_price).Value);
            pei.adjusted_price = int.Parse(itemElement.Attribute(PriceEstimateXmlNames._oadjust_adjustedprice).Value);
            pei.icann_fee = int.Parse(itemElement.Attribute(PriceEstimateXmlNames._icann_fee_adjusted).Value);
            responseData.Items.Add(pei);
          }
          catch (Exception ex)
          {
            var aex = new AtlantisException(requestData, "EcommPricingEstimateRequest",
                                            "Parsing XML response failed", ex.StackTrace + resultData.ToString());
            responseData = new EcommPricingEstimateResponseData(aex);
          }
        }
      }

      return responseData;
    }

    #endregion
  }
}
