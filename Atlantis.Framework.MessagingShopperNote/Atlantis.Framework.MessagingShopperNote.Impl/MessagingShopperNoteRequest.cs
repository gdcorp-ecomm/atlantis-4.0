using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MessagingShopperNote.Interface;

namespace Atlantis.Framework.MessagingShopperNote.Impl
{
  public class MessagingShopperNoteRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MessagingShopperNoteResponseData responseData = null;
      string responseXml = string.Empty;

      try
      {
        MessagingShopperNoteRequestData request = (MessagingShopperNoteRequestData)requestData;
        using (MessagingWS.WSCgdMessagingSystemService messagingWS = new MessagingWS.WSCgdMessagingSystemService())
        {
          messagingWS.Url = ((WsConfigElement)config).WSURL;
          messagingWS.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          responseXml = messagingWS.ProcessShopperMessage(request.ToXML());
          responseData = new MessagingShopperNoteResponseData(responseXml);
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new MessagingShopperNoteResponseData(responseXml, exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new MessagingShopperNoteResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }
  }
}
