using Atlantis.Framework.EcommInstorePending.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.EcommInstorePending.Impl
{
  public class EcommInstorePendingRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      try
      {
        EcommInstorePendingRequestData pendingRequest = (EcommInstorePendingRequestData)requestData;

        if (string.IsNullOrEmpty(pendingRequest.ShopperID))
        {
          result = new EcommInstorePendingResponseData((int)InstorePendingResult.EmptyShopper, "ShopperId missing", 0, "USD");
        }
        else if (string.IsNullOrEmpty(pendingRequest.TransactionalCurrencyType))
        {
          result = new EcommInstorePendingResponseData((int)InstorePendingResult.EmptyCurrency, "Currency missing", 0, "USD");
        }
        else
        {
          InstoreGrantService.Service instoreService = new InstoreGrantService.Service();
          instoreService.Url = ((WsConfigElement)config).WSURL;
          instoreService.Timeout = (int)pendingRequest.RequestTimeout.TotalMilliseconds;

          string xmlResult = instoreService.QueryAvailable(pendingRequest.ShopperID, pendingRequest.TransactionalCurrencyType);

          // example <ShopperISC result="2" shopper_id="855503" currency="USD" amount="0" errorDesc="No credits to consume"/>
          XElement resultElement = XElement.Parse(xmlResult);

          XAttribute resultAtt = resultElement.Attribute("result");
          XAttribute currencyAtt = resultElement.Attribute("currency");
          XAttribute amountAtt = resultElement.Attribute("amount");
          XAttribute messageAtt = resultElement.Attribute("errorDesc");

          if (resultAtt == null)
          {
            string message = "Unexpected output from service: " + xmlResult;
            throw new Exception(message);
          }

          string resultCodeText = resultAtt.Value;
          int resultCode;
          if (!int.TryParse(resultCodeText, out resultCode))
          {
            string message = "Unexpected output from service: " + xmlResult;
            throw new Exception(message);
          }

          if (resultCode == -1)
          {
            string message = "Unexpected error: " + xmlResult;
            throw new Exception(message);
          }

          string currency = (currencyAtt != null) ? currencyAtt.Value : "USD";
          string errorDesc = (messageAtt != null) ? messageAtt.Value : string.Empty;
          string amountText = (amountAtt != null) ? amountAtt.Value : string.Empty;

          int amount = 0;
          if (!string.IsNullOrEmpty(amountText))
          {
            if (!int.TryParse(amountText, out amount))
            {
              string message = "Unexpected amount from service: " + xmlResult;
              throw new Exception(message);
            }
          }

          result = new EcommInstorePendingResponseData(resultCode, errorDesc, amount, currency);
        }

      }
      catch (Exception ex)
      {
        result = new EcommInstorePendingResponseData(requestData, ex);
      }

      return result;
    }
  }
}
