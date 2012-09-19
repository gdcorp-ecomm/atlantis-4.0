using System;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoOrderLevelCreate.Interface;
using Atlantis.Framework.PromoOrderLevelCreate.Impl.WSgdPromoAPI;

namespace Atlantis.Framework.PromoOrderLevelCreate.Impl
{
    public class PromoOrderLevelCreate : IRequest
    {
      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {
        IResponseData result = null;
        PromoOrderLevelCreateRequestData request = null;
        string responseXml = null;

        try
        {
          request = requestData as PromoOrderLevelCreateRequestData;

          //validation for dates
          if (!request.PromoCode.SkipValidation)
          {
              if (!OrderLevelPromo.IsValidDate(request.PromoCode.StartDate))
              {
                  throw new OrderLevelPromoException(
                      "The promo start date format is invalid as it cannot be parsed to datetime format.",
                      new ArgumentException("OrderLevelPromo.StartDate"),
                      OrderLevelPromoExceptionReason.InvalidDateFormat);
              }

              if (!OrderLevelPromo.IsValidDate(request.PromoCode.EndDate))
              {
                  throw new OrderLevelPromoException(
                      "The promo end date format is invalid as it cannot be parsed to datetime format.",
                      new ArgumentException("OrderLevelPromo.EndDate"), OrderLevelPromoExceptionReason.InvalidDateFormat);
              }

              if (!OrderLevelPromo.IsDateInFuture(request.PromoCode.EndDate))
              {
                  throw new OrderLevelPromoException("The end date for a promo must be in the future.",
                                                     new ArgumentOutOfRangeException("OrderLevelPromo.EndDate"),
                                                     OrderLevelPromoExceptionReason.InvalidDateRange);
              }

              //Run currency validations
              OrderLevelPromoException validationException = null;
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
                throw new OrderLevelPromoException("The specified promo ID already exists", OrderLevelPromoExceptionReason.PromoAlreadyExists);
              }
              else if (errorDescNode.InnerXml.ToLower().Contains("xml load failed"))
              {
                throw new OrderLevelPromoException("The promo API was unable to parse the XML document passed to it. Please verify document consistency.", OrderLevelPromoExceptionReason.ImproperRequestFormat);
              }
              else
              {
                throw new OrderLevelPromoException(errorDescNode.InnerXml, OrderLevelPromoExceptionReason.Other);
              }
            }
          }
          result = new PromoOrderLevelCreateResponseData(request, responseXml);
        }
        catch (OrderLevelPromoException ex)
        {
          result = new PromoOrderLevelCreateResponseData(request, responseXml, ex);
        }
        catch (WebException ex)
        {
          result = new PromoOrderLevelCreateResponseData(request, ex.Status);
        }
        catch (AtlantisException ex)
        {
          result = new PromoOrderLevelCreateResponseData(request, responseXml, ex);
        }
        catch (Exception ex)
        {
          result = new PromoOrderLevelCreateResponseData(responseXml, request, ex);
        }

        return result;
      }
    }
}
