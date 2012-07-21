using System;
using System.Xml;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AddOrderLevelPromoPL.Interface;
using Atlantis.Framework.UpdateOrderPromoPL.Interface;
using Atlantis.Framework.UpdateOrderPromoPL.Impl.WSgdPromoAPI;

namespace Atlantis.Framework.UpdateOrderPromoPL.Impl
{
    public class UpdateOrderPromoPL : IRequest
    {
      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {
        IResponseData result = null;
        UpdateOrderPromoPLRequestData request = null;
        string responseXml = null;
        OrderLevelPromoVersion promo = null;
   
        try
        {
          request = requestData as UpdateOrderPromoPLRequestData;
          promo = request.PromoCode as OrderLevelPromoVersion;

          //Validation for dates
          if (!OrderLevelPromoVersion.IsValidDate(promo.StartDate))
          {
            throw new PLOrderLevelPromoException("The promo start date format is invalid as it cannot be parsed to datetime format.", new ArgumentException("OrderLevelPromoVersion.StartDate"), PLOrderLevelPromoExceptionReason.InvalidDateFormat);
          }

          if (!OrderLevelPromoVersion.IsValidDate(promo.EndDate))
          {
            throw new PLOrderLevelPromoException("The promo end date format is invalid as it cannot be parsed to datetime format.", new ArgumentException("OrderLevelPromoVersion.EndDate"), PLOrderLevelPromoExceptionReason.InvalidDateFormat);
          }

          if (!OrderLevelPromoVersion.IsDateInFuture(promo.EndDate))
          {
            throw new PLOrderLevelPromoException("The end date for a promo must be in the future.", new ArgumentOutOfRangeException("OrderLevelPromoVersion.EndDate"), PLOrderLevelPromoExceptionReason.InvalidDateRange);
          }

          //Currency Validations
          PLOrderLevelPromoException validationException = null;
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

          //Build the service call
          Service promoAPI = new Service();
          promoAPI.Url = ((WsConfigElement)config).WSURL;
          promoAPI.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          X509Certificate2 clientCertificate = ((WsConfigElement)config).GetClientCertificate();

          if (clientCertificate == null)
            throw new AtlantisException(request, "UpdateOrderPromoPL::RequestHandler", "No client certificate supplied to connect to the promoAPI webservice", null);

          //attach the client certificate (specified in atlantis.config)
          promoAPI.ClientCertificates.Add(clientCertificate);

          //make thhe physical call to the promoAPI (keeping fingers crossed).
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
                throw new PLOrderLevelPromoException("Invalid xml format in the request made to the promoAPI.", PLOrderLevelPromoExceptionReason.ImproperRequestFormat);
              }
              //identify if it's a duplicate record update
              else if (errorDescNode.InnerXml.ToLower().Contains("id specified already exists"))
              {
                throw new PLOrderLevelPromoException("The specified promo ID and version ID already exist in the promo database.", PLOrderLevelPromoExceptionReason.PromoAlreadyExists);
              }
              //identify if it's invalid version / ID combo
              else if (errorDescNode.InnerXml.ToLower().Contains("Invalid ID/Version specified"))
              {
                throw new PLOrderLevelPromoException("The specified promo ID/Version combination is invalid.", PLOrderLevelPromoExceptionReason.InvalidPromoGeneric);
              }
              else
              {
                throw new PLOrderLevelPromoException(errorDescNode.InnerXml, PLOrderLevelPromoExceptionReason.Other);
              }
            }
          }

          result = new UpdateOrderPromoPLResponseData(request, responseXml);

        }
        catch (PLOrderLevelPromoException ex)
        {
          result = new UpdateOrderPromoPLResponseData(request, ex);
        }
        catch (AtlantisException ex)
        {
          result = new UpdateOrderPromoPLResponseData(request, responseXml, ex);
        }
        catch (WebException ex)
        {
          result = new UpdateOrderPromoPLResponseData(request, ex.Status);
        }
        catch (Exception ex)
        {
          result = new UpdateOrderPromoPLResponseData(request, responseXml, ex);
        }

        return result;
      }
    }
}
