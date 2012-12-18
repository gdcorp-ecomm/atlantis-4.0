using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.BonsaiPurchase.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiPurchase.Impl
{
    public class BonsaiPurchaseRequest : IRequest
    {
      public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
      {
        var responseData = new BonsaiPurchaseResponseData();
        var requestData = (BonsaiPurchaseRequestData)oRequestData;
        var configuration = (WsConfigElement)oConfig;

        using (var bpService = new BonsaiPurchase.Service())
        {
          bpService.Url = configuration.WSURL;
          bpService.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;

          X509Certificate2 cert = configuration.GetClientCertificate();
          cert.Verify();
          bpService.ClientCertificates.Add(cert);
        
          try
          {
            int resultCode;
            string errorText;
            string orderXml;
            var basketCode = bpService.PurchaseChangeAccountRequest(requestData.ResourceID, requestData.ResourceType, requestData.IDType,
                                            requestData.AccountPurchaseChangeXML,
                                            requestData.RenewalPFID, requestData.RenewalPeriods,
                                            requestData.ItemRequestXML, out resultCode, out errorText, out orderXml);

            if (resultCode < 0 || basketCode < 0)
            {
              responseData.AtlException = CreateAtlantisException(requestData, null,
                                                                  string.Format("ResultCode: {0}, BasketCode: {1}, ErrorText: {2}",
                                                                    resultCode, basketCode, errorText));
              responseData.IsSuccess = false;
            }
            else
            {
              responseData.BasketResultCode = basketCode;
              responseData.IsSuccess = true;
              responseData.OrderXml = orderXml;
            }
          }
          catch (Exception ex)
          {
            responseData.AtlException = CreateAtlantisException(requestData, ex, "--"); 
            responseData.IsSuccess = false;
          }
        }
        return responseData;
      }

      private static AtlantisException CreateAtlantisException(BonsaiPurchaseRequestData requestData, Exception ex, string errorMessage)
      {
        string data = string.Format("ResourceID: {0},"
                                    + " ResourceType: {1}, IDType: {2}, AccountChangeXML: {3}, RenewalPFID: {4},"
                                    + " RenewalPeriods: {5}, ItemRequestXML: {6}, errorMessage: {7}",
                                    requestData.ResourceID, requestData.ResourceType, requestData.IDType,
                                    requestData.AccountPurchaseChangeXML, requestData.RenewalPFID,
                                    requestData.RenewalPeriods, requestData.ItemRequestXML, errorMessage);
        var atlException = new AtlantisException(requestData, "BonsaiPurchaseRequest.RequestHandler",
                                                 "Failure processing instant purchase", data, ex);
        return atlException;
      }
    }
}
