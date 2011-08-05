using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MessagingProcess.Interface;

namespace Atlantis.Framework.MessagingProcess.Impl
{
  public class MessagingProcessRequest : IRequest
  {

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      MessagingProcessResponseData response = null;
      string responseXml = string.Empty;

      try
      {
        MessagingProcessRequestData request = (MessagingProcessRequestData)oRequestData;

        using (MessagingWS.WSCgdMessagingSystemService messagingWS = new MessagingWS.WSCgdMessagingSystemService())
        {
          messagingWS.Url = ((WsConfigElement)oConfig).WSURL;
          messagingWS.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          responseXml = messagingWS.ProcessXml(request.ToXML());
        }

        response = new MessagingProcessResponseData(responseXml);
      }
      catch (Exception ex)
      {
        response = new MessagingProcessResponseData(responseXml, oRequestData, ex);
      }

      return response;
    }

    #endregion
  }
}
