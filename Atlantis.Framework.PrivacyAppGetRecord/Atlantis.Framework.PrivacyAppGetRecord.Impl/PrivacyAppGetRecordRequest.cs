using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivacyAppGetRecord.Impl.privacyWS;
using Atlantis.Framework.PrivacyAppGetRecord.Interface;

namespace Atlantis.Framework.PrivacyAppGetRecord.Impl
{
  public class PrivacyAppGetRecordRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;

      string responseXml = string.Empty;
      wscgdPrivacyAppService service = null;

      try
      {
        PrivacyAppGetRecordRequestData request = (PrivacyAppGetRecordRequestData)oRequestData;

        service = new wscgdPrivacyAppService();
        service.Url = ((WsConfigElement)oConfig).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        service.GetRecord(request.HashKey, request.ApplicationId, out responseXml);

        result = new PrivacyAppGetRecordResponseData(responseXml);

      }
      catch (AtlantisException exAtlantis)
      {
        result = new PrivacyAppGetRecordResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        result = new PrivacyAppGetRecordResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }

      return result;
    }

    #endregion
  }
}
