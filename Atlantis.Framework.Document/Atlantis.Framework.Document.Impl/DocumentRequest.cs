using System;

using Atlantis.Framework.Document.Impl.DocumentWS;
using Atlantis.Framework.Document.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Document.Impl
{
  public class DocumentRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      string html = null;

      try
      {
        var request = (DocumentRequestData)oRequestData;
        using (var documentService = new DocumentService())
        {
          documentService.Url = ((WsConfigElement)oConfig).WSURL;
          documentService.Timeout = (int) request.RequestTimeout.TotalMilliseconds;
          html = documentService.document(request.ToXML());
        }

        oResponseData = new DocumentResponseData(html);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DocumentResponseData(html, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new DocumentResponseData(html, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
