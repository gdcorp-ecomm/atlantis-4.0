using System;

using Atlantis.Framework.DocumentAttributes.Impl.gdg.intranet.dev.gdhelp.api;
using Atlantis.Framework.DocumentAttributes.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DocumentAttributes.Impl
{
  public class DocumentAttributesRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData response = null;
      string xml = null;

      try
      {
        DocumentAttributesRequestData request = (DocumentAttributesRequestData)requestData;
        using (DocumentService documentService = new DocumentService())
        {
          documentService.Url = ((WsConfigElement)config).WSURL;
          documentService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          xml = documentService.documentAttributes(request.ToXML());
        }

        if (string.IsNullOrEmpty(xml))
        {
          throw new Exception("DocumentAttributes returned null response.");
        }
        response = new DocumentAttributesResponseData(xml);
      }
      catch (AtlantisException exAtlantis)
      {
        response = new DocumentAttributesResponseData(xml, exAtlantis);
      }
      catch (Exception ex)
      {
        response = new DocumentAttributesResponseData(xml, requestData, ex);
      }

      return response;

    }

    #endregion
  }
}
