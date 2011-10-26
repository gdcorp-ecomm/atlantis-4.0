using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MessagingProcess.Interface;
using Atlantis.Framework.PurchaseEmail.Interface;

namespace Atlantis.Framework.PurchaseEmail.Impl
{
  public class PurchaseEmailRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      PurchaseEmailResponseData result = new PurchaseEmailResponseData();
      PurchaseEmailRequestData request = oRequestData as PurchaseEmailRequestData;
      AtlantisException messageRequestException;
      List<MessagingProcessRequestData> messageRequests = request.GetPurchaseConfirmationEmailRequests(out messageRequestException);
      if (messageRequestException != null)
      {
        result.AddException(messageRequestException);
      }

      if (messageRequests != null)
      {
        foreach (MessagingProcessRequestData messageRequest in messageRequests)
        {
          try
          {
            MessagingProcessResponseData response =
              (MessagingProcessResponseData)Engine.Engine.ProcessRequest(messageRequest, PurchaseEmailEngineRequests.MessagingProcess);
            result.AddMessageResponse(response);
          }
          catch (Exception ex)
          {
            result.AddException(ex);
          }
        }
      }

      return result;
    }

    #endregion
  }
}
