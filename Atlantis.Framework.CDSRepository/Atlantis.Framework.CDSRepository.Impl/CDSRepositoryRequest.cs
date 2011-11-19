using System;
using Atlantis.Framework.CDSRepository.Impl.RepositoryHelpers;
using Atlantis.Framework.CDSRepository.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CDSRepository.Impl
{

  public class CDSRepositoryRequest : IRequest
  {
    IDocumentRepository DocumentRepository { get; set; }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CDSRepositoryResponseData responseData = null;
      var cdsRequestData = requestData as CDSRepositoryRequestData;
      try
      {
        if (cdsRequestData == null)
        {
          throw new Exception("CDSRepositoryRequestData requestData is null");
        }

        var result = string.Empty;

        string rootPath = config.GetConfigValue("RootPath");
        DocumentRepository = new FlatFileDocumentRepository(rootPath);
        result = DocumentRepository.GetDocument(cdsRequestData.Query);

        if (!string.IsNullOrWhiteSpace(result))
        {
          responseData = new CDSRepositoryResponseData(result);
        }
        else
        {
          var aex = new AtlantisException(
            requestData, "CDSRepositoryRequest.RequestHandler",
            "Request returned empty json.", cdsRequestData.Query);
          responseData = new CDSRepositoryResponseData(aex);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        string data = cdsRequestData.Query;
        var aex = new AtlantisException(requestData, "CDSRepositoryRequest.RequestHandler", message, data);
        responseData = new CDSRepositoryResponseData(aex);
      }

      return responseData;
    }
  }
}
