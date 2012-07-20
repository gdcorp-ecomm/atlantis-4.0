using System;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AddOrderLevelPromoPL.Interface;
using Atlantis.Framework.AddOrderLevelPromoPL.Impl.WSgdPromoAPI;

namespace Atlantis.Framework.AddOrderLevelPromoPL.Impl
{
    public class AddOrderLevelPromoPL : IRequest
    {
      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {
        IResponseData result = null;
        PLOrderLevelPromoRequestData request = null;
        string responseXml = null;

        try
        {
          request = requestData as PLOrderLevelPromoRequestData;

          //validation for dates
          if (!OrderLevelPromo.IsValidDate(request.PromoCode.StartDate))
          {
            throw new PLOrderLevelPromoException("The promo start date format is invalid as it cannot be parsed to datetime format.", new ArgumentException("OrderLevelPromo.StartDate"), PLOrderLevelPromoExceptionReason.InvalidDateFormat);
          }

          if (!OrderLevelPromo.IsValidDate(request.PromoCode.EndDate))
          {
            throw new  PLOrderLevelPromoException("The promo end date format is invalid as it cannot be parsed to datetime format.", new ArgumentException("OrderLevelPromo.EndDate"), PLOrderLevelPromoExceptionReason.InvalidDateFormat);
          }

          if (!OrderLevelPromo.IsDateInFuture(request.PromoCode.EndDate))
          {
            throw new PLOrderLevelPromoException("The end date for a promo must be in the future.", new ArgumentOutOfRangeException("OrderLevelPromo.EndDate"), PLOrderLevelPromoExceptionReason.InvalidDateRange);
          }

          //Run currency validations
          PLOrderLevelPromoException validationException = null;
          foreach (PrivateLabelPromoCurrency currency in request.PromoCode.Currencies)
          {
            if (!PrivateLabelPromoCurrency.IsPromoCurrencyValid(currency, ref validationException))
            {
              if (validationException != null)
              {
                throw validationException;
              }
            }
          }

          Service service = new Service();
          service.Url = ((WsConfigElement)config).WSURL;
          service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          X509Certificate2 clientCert = ((WsConfigElement)config).GetClientCertificate();

          if (clientCert == null)
            throw new AtlantisException(request, "AddOrderLevelPromoPL::RequestHandler", "Invalid or missing client certificate for the web service.", null);

          service.ClientCertificates.Add(clientCert);

          responseXml = service.AddOrderPromoXML(request.ToXML());

          if (string.IsNullOrEmpty(responseXml))
            throw new Exception("WSgdPromoAPI returned a null or empty response to the method call 'AddOrderPromoXML'");

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

            //identify if it's a duplicate record issue
            if (errorDescNode != null)
            {
              if (errorDescNode.InnerXml.ToLower().Contains("id specified already exists"))
              {
                throw new PLOrderLevelPromoException("TEST", PLOrderLevelPromoExceptionReason.PromoAlreadyExists);
              }
              else if (errorDescNode.InnerXml.ToLower().Contains("xml load failed"))
              {
                throw new PLOrderLevelPromoException(errorDescNode.InnerXml, PLOrderLevelPromoExceptionReason.ImproperRequestFormat);
              }
              else
              {
                throw new PLOrderLevelPromoException(errorDescNode.InnerXml, PLOrderLevelPromoExceptionReason.Other);
              }
            }
          }
          result = new PLOrderLevelPromoResponseData(request, responseXml);
        }
        catch (PLOrderLevelPromoException ex)
        {
          result = new PLOrderLevelPromoResponseData(request, responseXml, ex);
        }
        catch (WebException ex)
        {
          result = new PLOrderLevelPromoResponseData(request, ex.Status);
        }
        catch (AtlantisException ex)
        {
          result = new PLOrderLevelPromoResponseData(request, responseXml, ex);
        }
        catch (Exception ex)
        {
          result = new PLOrderLevelPromoResponseData(responseXml, request, ex);
        }

        return result;
      }
    }
}
