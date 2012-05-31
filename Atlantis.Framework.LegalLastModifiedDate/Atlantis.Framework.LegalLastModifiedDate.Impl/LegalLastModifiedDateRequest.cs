using System;
using Atlantis.Framework.LegalLastModifiedDate.Impl.DocumentWS;
using Atlantis.Framework.LegalLastModifiedDate.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LegalLastModifiedDate.Impl
{
  public class LegalLastModifiedDateRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      LegalLastModifiedDateResponseData oResponseData;

      try
      {
        LegalLastModifiedDateRequestData request = (LegalLastModifiedDateRequestData)oRequestData;
        HelpCenter service = new HelpCenter();
        service.Url = ((WsConfigElement)oConfig).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        string progId = DataCache.DataCache.GetProgID(request.PrivateLabelId);
        DateTime lastModified = service.get_legal_agreement_last_modified(request.PageId, progId);
        oResponseData = new LegalLastModifiedDateResponseData(lastModified);
      }
      catch (Exception ex)
      {
        oResponseData = new LegalLastModifiedDateResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
