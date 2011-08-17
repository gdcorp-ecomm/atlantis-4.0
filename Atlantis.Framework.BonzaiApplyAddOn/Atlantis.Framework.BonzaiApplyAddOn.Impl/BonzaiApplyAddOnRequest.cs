using System;
using Atlantis.Framework.BonzaiApplyAddOn.Impl.BonzaiWebService;
using Atlantis.Framework.BonzaiApplyAddOn.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonzaiApplyAddOn.Impl
{
  public class BonzaiApplyAddOnRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      WebServiceResponse wsResponse;
      BonzaiApplyAddOnResponseData responseData;

      try
      {
        var bonzaiRequestData = (BonzaiApplyAddOnRequestData)requestData;

        using (var bonsaiWs = new Service())
        {
          bonsaiWs.Url = ((WsConfigElement) config).WSURL;
          bonsaiWs.Timeout = (int) bonzaiRequestData.RequestTimeout.TotalMilliseconds;
          wsResponse = bonsaiWs.ApplyAddOn(bonzaiRequestData.ShopperID, bonzaiRequestData.AccountUid, bonzaiRequestData.AddOnType);
        }

        if (wsResponse.ResultCode == 0)
        {
          responseData = new BonzaiApplyAddOnResponseData();
        }
        else
        {
          throw new AtlantisException(requestData,
                                      "BonzaiApplyAddOnRequest.RequestHandler",
                                      "Invalid BonzaiWebservice Request",
                                      string.Format("ResponseCode: {0} -- {1}", wsResponse.ResultCode, TranslateResponse(wsResponse.ResultCode)));
        }
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new BonzaiApplyAddOnResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new BonzaiApplyAddOnResponseData(requestData, ex);
      }
       
      return responseData;
    }

    #region Error Helper Method
    private static string TranslateResponse(int responseCode)
    {
      string responseString = string.Empty;

      switch (responseCode)
      {
        case -1:
          responseString = "Invalid Database Connection";
          break;
        case -100:
          responseString = "No Resource Available";
          break;
        case -101:
          responseString = "Unable To Update Resource";
          break;
        case -103:
          responseString = "Unable To Obtain Resource Data For Shopper";
          break;
        case -200:
          responseString = "Orion Webservice Call Failed";
          break;
        case -201:
          responseString = "Unable To Add Attribute";
          break;
        case -203:
          responseString = "Orion AddOn Already Exists";
          break;
        case -300:
          responseString = "Missing Shopper ID";
          break;
        case -301:
          responseString = "Missing Account UID";
          break;
        case -302:
          responseString = "Missing AddOn Type";
          break;
        case -303:
          responseString = "AddOn Type Not Supported";
          break;
        default:
          break;
      }

      return responseString;
    }
    #endregion
  }
}
