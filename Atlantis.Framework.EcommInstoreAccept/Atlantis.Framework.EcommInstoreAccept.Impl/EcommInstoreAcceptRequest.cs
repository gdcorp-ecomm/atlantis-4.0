using Atlantis.Framework.EcommInstoreAccept.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.EcommInstoreAccept.Impl
{
  public class EcommInstoreAcceptRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      try
      {
        EcommInstoreAcceptRequestData acceptRequest = (EcommInstoreAcceptRequestData)requestData;

        if (string.IsNullOrEmpty(acceptRequest.ShopperID))
        {
          result = new EcommInstoreAcceptResponseData((int)InstoreAcceptResult.EmptyShopper, "ShopperId missing");
        }

        InstoreGrantService.Service instoreService = new InstoreGrantService.Service();
        instoreService.Url = ((WsConfigElement)config).WSURL;
        instoreService.Timeout = (int)acceptRequest.RequestTimeout.TotalMilliseconds;

        string xmlResult = instoreService.Accept(acceptRequest.ShopperID);

        // example <ShopperISC result="2" errorDesc="No credits to consume"/>
        XElement resultElement = XElement.Parse(xmlResult);

        XAttribute resultAtt = resultElement.Attribute("result");
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

        string errorDesc = (messageAtt != null) ? messageAtt.Value : string.Empty;
        result = new EcommInstoreAcceptResponseData(resultCode, errorDesc);
      }
      catch(Exception ex)
      {
        result = new EcommInstoreAcceptResponseData(requestData, ex);
      }

      return result;
    }
  }
}
