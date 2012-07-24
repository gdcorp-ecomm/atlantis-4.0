using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoOrderLevelAddPL.Interface;
using Atlantis.Framework.PromoOrderLevelAddPL.Impl.WSgdPromoAPI;

namespace Atlantis.Framework.PromoOrderLevelAddPL.Impl
{
  public class PromoOrderLevelAddPL : IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement configElement)
    {
      IResponseData responseData = null;
      string responseXml = string.Empty;

      try
      {
        PromoOrderLevelAddPLRequestData resellerRequestData = requestData as PromoOrderLevelAddPLRequestData;

        PrivateLabelPromoException plPromoException = null;
        foreach (PrivateLabelPromo promo in resellerRequestData.ResellerPromoList.Values)
        {
          if (!PrivateLabelPromo.ValidatePrivateLabelPromo(promo, ref plPromoException))
          {
            if (plPromoException != null)
            {
              throw plPromoException;
            }
            else
            {
              throw new PrivateLabelPromoException("The PrivateLabelPromo object for PLID [" + promo.PrivateLabelId + "] has invalid data.", PrivateLabelPromoExceptionReason.Unknown);
            }
          }
        }

        Service service = new Service();
        service.Url = ((WsConfigElement)configElement).WSURL;
        service.Timeout = (int)resellerRequestData.RequestTimeout.TotalMilliseconds;

        X509Certificate2 clientCert = ((WsConfigElement)configElement).GetClientCertificate();

        if (clientCert == null)
        {
          throw new AtlantisException(requestData, "CreateResellerOrderLevelPromoRequest::RequestHandler", "Missing or invalid client certificate for the request.", ((WsConfigElement)configElement).WSURL);
        }

        service.ClientCertificates.Add(clientCert);

        //make the request
        responseXml = service.AddOrderPromoPrivateLabel(resellerRequestData.ToXML());

        if (responseXml == null)
        {
          throw new Exception("WSgdPromoAPI returned a null response to the method call 'AddOrderPromoPrivateLabel'.");
        }

        //look for exceptions returned in the xml response
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(responseXml);

        XmlNode rootNode = xmlDoc.DocumentElement;
        XmlNode searchNode = rootNode.SelectSingleNode("//Error");

        //If error is returned, parse it out for the response object
        if (searchNode != null)
        {
          XmlNode errorDescNode = rootNode.SelectSingleNode("//Description");

          //look for invalid request format
          if (errorDescNode.InnerXml.ToLower().Contains("xml load failed"))
          {
            responseData = new PromoOrderLevelAddPLResponseData(requestData, responseXml, new PrivateLabelPromoException("Invalid formatting of XML request", PrivateLabelPromoExceptionReason.InvalidRequestFormat));
          }
          //look for invalid (doesn't exist in db) promo code
          else if (errorDescNode.InnerXml.ToLower().Contains("invalid  promo id"))
          {
            throw new PrivateLabelPromoException("Invalid Promo Code - the code specified does not exist in the database.", PrivateLabelPromoExceptionReason.PromoCodeDoesntExist);
          }
          else
          {
            throw new PrivateLabelPromoException("Error processing the request.", PrivateLabelPromoExceptionReason.Unknown);
          }
        }

      responseData = new PromoOrderLevelAddPLResponseData(resellerRequestData, responseXml);
      }
      catch (PrivateLabelPromoException e)
      {
        responseData = new PromoOrderLevelAddPLResponseData(requestData, responseXml, e);
      }
      catch (AtlantisException e)
      {
        responseData = new PromoOrderLevelAddPLResponseData(requestData, responseXml, e);
      }
      catch (WebException e)
      {
        responseData = new PromoOrderLevelAddPLResponseData(requestData, e.Status);
      }
      catch (Exception e)
      {
        responseData = new PromoOrderLevelAddPLResponseData(responseXml, requestData, e);
      }

      return responseData;
    }

  }
}
