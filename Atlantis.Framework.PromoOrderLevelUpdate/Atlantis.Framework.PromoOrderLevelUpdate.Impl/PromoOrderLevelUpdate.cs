using System;
using System.Xml;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoOrderLevelCreate.Interface;
using Atlantis.Framework.PromoOrderLevelUpdate.Interface;
using Atlantis.Framework.PromoOrderLevelUpdate.Impl.WSgdPromoAPI;

namespace Atlantis.Framework.PromoOrderLevelUpdate.Impl
{
    public class PromoOrderLevelUpdate : IRequest
    {
      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {
        IResponseData result = null;
        PromoOrderLevelUpdateRequestData request = null;
        string responseXml = null;
        OrderLevelPromoVersion promo = null;
        string promoVersion = null;
   
        try
        {
          request = requestData as PromoOrderLevelUpdateRequestData;
          promo = request.PromoCode as OrderLevelPromoVersion;
          request.PromoCode.ISCCode = config.GetConfigValue("CustomPromoTrackingCode");
          //Validation for dates
          if (!request.PromoCode.SkipValidation)
          {
              if (!OrderLevelPromoVersion.IsValidDate(promo.StartDate))
              {
                  throw new OrderLevelPromoException(
                      "The promo start date format is invalid as it cannot be parsed to datetime format.",
                      new ArgumentException("OrderLevelPromoVersion.StartDate"),
                      OrderLevelPromoExceptionReason.InvalidDateFormat);
              }

              if (!OrderLevelPromoVersion.IsValidDate(promo.EndDate))
              {
                  throw new OrderLevelPromoException(
                      "The promo end date format is invalid as it cannot be parsed to datetime format.",
                      new ArgumentException("OrderLevelPromoVersion.EndDate"),
                      OrderLevelPromoExceptionReason.InvalidDateFormat);
              }

              if (!OrderLevelPromoVersion.IsDateInFuture(promo.EndDate))
              {
                  throw new OrderLevelPromoException("The end date for a promo must be in the future.",
                                                     new ArgumentOutOfRangeException("OrderLevelPromoVersion.EndDate"),
                                                     OrderLevelPromoExceptionReason.InvalidDateRange);
              }

              //Currency Validations
              OrderLevelPromoException validationException = null;
              foreach (PrivateLabelPromoCurrency currency in promo.Currencies)
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

            //Build the service call
          Service promoAPI = new Service();
          promoAPI.Url = ((WsConfigElement)config).WSURL;
          promoAPI.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          X509Certificate2 clientCertificate = ((WsConfigElement)config).GetClientCertificate();

          if (clientCertificate == null)
            throw new AtlantisException(request, "UpdateOrderPromoPL::RequestHandler", "No client certificate supplied to connect to the promoAPI webservice", null);

          //attach the client certificate (specified in atlantis.config)
          promoAPI.ClientCertificates.Add(clientCertificate);

          //first make call to get and parse out current version (if the promo exists)
          string currentVersion = promoAPI.GetOrderPromoByID(promo.PromoId);

          if (string.IsNullOrEmpty(currentVersion))
          {
            throw new OrderLevelPromoException("The order level promo cannot be updated because it does not currently exist in the promo system.", OrderLevelPromoExceptionReason.InvalidPromoGeneric);
          }
          else
          {
            XmlDocument existingResponseDoc = new XmlDocument();
            existingResponseDoc.LoadXml(currentVersion);
            XmlNode root = existingResponseDoc.DocumentElement;
            XmlNode promoNode = root.SelectSingleNode("//OrderPromo");

            promoVersion = promoNode.Attributes["version"].Value;
          }

          //Update the version
          ((OrderLevelPromoVersion)request.PromoCode).VersionId = promoVersion;

          //make thhe physical call to the promoAPI 
          responseXml = promoAPI.UpdateOrderPromoXML(request.ToXML());

          //handle empty response
          if (string.IsNullOrEmpty(responseXml))
          {
            throw new Exception("WSgdPromoAPI returned a null or empty response to the method call 'UpdateOrderPromoXML'");
          }

          //handle errors returned in response
          XmlDocument responseDoc = new XmlDocument();
          responseDoc.LoadXml(responseXml);
          XmlNode rootNode = responseDoc.DocumentElement;
          XmlNode errorNode = rootNode.SelectSingleNode("//Error");
          XmlNode errorDescNode = null;

          if (errorNode != null)
          {
            errorDescNode = rootNode.SelectSingleNode("//Description");

            if (errorDescNode != null)
            {
              //identify if it's invalid request xml format
              if (errorDescNode.InnerXml.ToLower().Contains("xml load failed"))
              {
                throw new OrderLevelPromoException("Invalid xml format in the request made to the promoAPI.", OrderLevelPromoExceptionReason.ImproperRequestFormat);
              }
              //identify if it's a duplicate record update
              else if (errorDescNode.InnerXml.ToLower().Contains("id specified already exists"))
              {
                throw new OrderLevelPromoException("The specified promo ID and version ID already exist in the promo database.", OrderLevelPromoExceptionReason.PromoAlreadyExists);
              }
              //identify if it's invalid version / ID combo
              else if (errorDescNode.InnerXml.ToLower().Contains("Invalid ID/Version specified"))
              {
                throw new OrderLevelPromoException("The specified promo ID/Version combination is invalid.", OrderLevelPromoExceptionReason.InvalidPromoGeneric);
              }
              else
              {
                throw new OrderLevelPromoException(errorDescNode.InnerXml, OrderLevelPromoExceptionReason.Other);
              }
            }
          }

          result = new PromoOrderLevelUpdateResponseData(request, responseXml);

        }
        catch (OrderLevelPromoException ex)
        {
          result = new PromoOrderLevelUpdateResponseData(request, ex);
        }
        catch (AtlantisException ex)
        {
          result = new PromoOrderLevelUpdateResponseData(request, responseXml, ex);
        }
        catch (WebException ex)
        {
          result = new PromoOrderLevelUpdateResponseData(request, ex.Status);
        }
        catch (Exception ex)
        {
          result = new PromoOrderLevelUpdateResponseData(request, responseXml, ex);
        }

        return result;
      }

    }
}
