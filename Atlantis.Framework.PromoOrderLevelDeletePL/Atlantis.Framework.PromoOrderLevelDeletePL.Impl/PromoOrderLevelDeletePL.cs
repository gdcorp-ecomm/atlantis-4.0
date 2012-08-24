using System;
using System.Xml;
using System.Net;
using System.Security.Cryptography.X509Certificates;

using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoOrderLevelDeletePL.Interface;
using Atlantis.Framework.PromoOrderLevelDeletePL.Impl.WSgdPromoAPI;

namespace Atlantis.Framework.PromoOrderLevelDeletePL.Impl
{
    public class PromoOrderLevelDeletePL : IRequest
    {
      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {
        PromoOrderLevelDeletePLResponseData result = null;
        PromoOrderLevelDeletePLRequestData request = null;
        string responseXml = null;

        try
        {
          request = requestData as PromoOrderLevelDeletePLRequestData;

          Service service = new Service();
          service.Url = ((WsConfigElement)config).WSURL;
          service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          X509Certificate2 clientCert = ((WsConfigElement)config).GetClientCertificate();

          if (clientCert == null)
          {
            throw new AtlantisException(request, "PromoOrderLevelDeletePL::RequestHandler", "Invalid or missing client certificate", string.Empty);
          }

          service.ClientCertificates.Add(clientCert);
          responseXml = service.DeleteOrderPromoPrivateLabel(request.ToXML());

          if (string.IsNullOrEmpty(responseXml))
          {
            throw new Exception("WSgdPromoAPI returned a null or empty response to the method call 'DeleteOrderPromoPrivateLabel'");
          }

          XmlDocument responseDoc = new XmlDocument();
          responseDoc.LoadXml(responseXml);

          XmlNode rootNode = responseDoc.DocumentElement;
          XmlNode errorNode = rootNode.SelectSingleNode("//Error");
          XmlNode errorDescNode;

          //Checking if there's an error record 
          if (errorNode != null)
          {
            //get error message
            errorDescNode = rootNode.SelectSingleNode("//Description");

            if (errorDescNode == null)
            {
              throw new AtlantisException(request, "PromoOrderLevelDeletePL::RequestHandler", "The promo API service was unable to process the request", errorDescNode.InnerText);
            }
            else
            {
              throw new AtlantisException(request, "PromoOrderLevelDeletePL::RequestHandler", errorDescNode.InnerText, responseXml);
            }
          }
          else
          {
            result = new PromoOrderLevelDeletePLResponseData(request, responseXml);
          }
        }
        catch (AtlantisException ex)
        {
          result = new PromoOrderLevelDeletePLResponseData(request, responseXml, ex);
        }
        catch (WebException ex)
        {
          result = new PromoOrderLevelDeletePLResponseData(request, ex.Status);
        }
        catch (Exception ex)
        {
          result = new PromoOrderLevelDeletePLResponseData(request, responseXml, ex);
        }

        return result;
      }
    }
}
