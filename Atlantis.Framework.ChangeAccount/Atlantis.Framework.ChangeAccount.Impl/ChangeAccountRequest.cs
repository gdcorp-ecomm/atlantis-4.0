using System;

using Atlantis.Framework.ChangeAccount.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ChangeAccount.Impl
{
  public class ChangeAccountRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData returnResponse;
      var requestData = (ChangeAccountRequestData)oRequestData;

      if (requestData.ChangeAccountRequests != null)
      {
        returnResponse = BatchRequestHandler(oRequestData, oConfig);
      }
      else
      {
        returnResponse = SingleRequestHandler(oRequestData, oConfig);
      }

      return returnResponse;
    }

    public IResponseData BatchRequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      var responseData = new ChangeAccountResponseData { IsSuccess = true };
      var requestData = (ChangeAccountRequestData)oRequestData;
      var configuration = (WsConfigElement)oConfig;

      using (var manager = new BonsaiManager.Service())
      {
        manager.Url = configuration.WSURL;
        manager.Timeout = (int) requestData.RequestTimeout.TotalMilliseconds;

        foreach (var requestObject in requestData.ChangeAccountRequests)
        {
          var responseObject = new ChangeAccountResponseObject
                                 {
                                   RenewalPFID = requestObject.RenewalPFID,
                                   ResourceID = requestObject.ResourceID,
                                 };
          try
          {
            int resultCode;

            int basketCode = manager.ChangeAccountRequest(requestObject.ResourceID, requestObject.ResourceType,
                                                          requestObject.IDType, requestObject.AccountChangeXML,
                                                          requestObject.RenewalPFID,
                                                          requestObject.RenewalPeriods, requestObject.ItemRequestXML,
                                                          out resultCode);
            if (resultCode < 0 || basketCode < 0)
            {
              string data = string.Format("Result Code: {0}, Basket Result Code {1}, ResourceID: {2},"
                                          + " ResourceType: {3}, IDType: {4}, AccountChangeXML: {5}, RenewalPFID: {6},"
                                          + " RenewalPeriods: {7}, ItemRequestXML: {8}",
                                          resultCode, basketCode, requestObject.ResourceID,
                                          requestObject.ResourceType, requestObject.IDType,
                                          requestObject.AccountChangeXML,
                                          requestObject.RenewalPFID, requestObject.RenewalPeriods,
                                          requestObject.ItemRequestXML);
              responseObject.AtlException = new AtlantisException(requestData,
                                                                  "ChangeAccountRequest.RequestHandler",
                                                                  "Failure changing account", data);
              responseObject.IsSuccess = false;
              responseData.IsSuccess = false;
            }
            else
            {
              responseObject.BasketResultCode = basketCode;
              responseObject.IsSuccess = true;
            }
          }
          catch (Exception ex)
          {
            string data = string.Format("ResourceID: {0},"
                                        + " ResourceType: {1}, IDType: {2}, AccountChangeXML: {3}, RenewalPFID: {4},"
                                        + " RenewalPeriods: {5}, ItemRequestXML: {6}",
                                        requestObject.ResourceID, requestObject.ResourceType, requestObject.IDType,
                                        requestObject.AccountChangeXML, requestObject.RenewalPFID,
                                        requestObject.RenewalPeriods, requestObject.ItemRequestXML);
            responseObject.AtlException = new AtlantisException(requestData,
                                                                "ChangeAccountRequest.RequestHandler",
                                                                "Failure changing account", data, ex);
            responseObject.IsSuccess = false;
            responseData.IsSuccess = false;
          }
          responseData.ChangeAccountResponses.Add(responseObject);
        }
      }

      return responseData;
    }

    public IResponseData SingleRequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      var responseData = new ChangeAccountResponseData();
      var requestData = (ChangeAccountRequestData)oRequestData;
      var configuration = (WsConfigElement)oConfig;

      using (var manager = new BonsaiManager.Service())
      {
        manager.Url = configuration.WSURL;
        manager.Timeout = (int) requestData.RequestTimeout.TotalMilliseconds;

        try
        {
          int resultCode;
          int basketCode = manager.ChangeAccountRequest(requestData.ResourceID, requestData.ResourceType,
                                                        requestData.IDType, requestData.AccountChangeXML,
                                                        requestData.RenewalPFID,
                                                        requestData.RenewalPeriods, requestData.ItemRequestXML,
                                                        out resultCode);
          if (resultCode < 0 || basketCode < 0)
          {
            string data = string.Format("Result Code: {0}, Basket Result Code {1}, ResourceID: {2},"
                                        + " ResourceType: {3}, IDType: {4}, AccountChangeXML: {5}, RenewalPFID: {6},"
                                        + " RenewalPeriods: {7}, ItemRequestXML: {8}",
                                        resultCode, basketCode, requestData.ResourceID,
                                        requestData.ResourceType, requestData.IDType, requestData.AccountChangeXML,
                                        requestData.RenewalPFID, requestData.RenewalPeriods,
                                        requestData.ItemRequestXML);
            responseData.AtlException = new AtlantisException(requestData,
                                                              "ChangeAccountRequest.RequestHandler",
                                                              "Failure changing account", data);
            responseData.IsSuccess = false;
          }
          else
          {
            responseData.BasketResultCode = basketCode;
            responseData.IsSuccess = true;
          }
        }
        catch (Exception ex)
        {
          string data = string.Format("ResourceID: {0},"
                                      + " ResourceType: {1}, IDType: {2}, AccountChangeXML: {3}, RenewalPFID: {4},"
                                      + " RenewalPeriods: {5}, ItemRequestXML: {6}",
                                      requestData.ResourceID, requestData.ResourceType, requestData.IDType,
                                      requestData.AccountChangeXML, requestData.RenewalPFID,
                                      requestData.RenewalPeriods, requestData.ItemRequestXML);
          responseData.AtlException = new AtlantisException(requestData,
                                                            "ChangeAccountRequest.RequestHandler",
                                                            "Failure changing account", data, ex);
          responseData.IsSuccess = false;
        }
      }

      return responseData;
    }

  }
}
