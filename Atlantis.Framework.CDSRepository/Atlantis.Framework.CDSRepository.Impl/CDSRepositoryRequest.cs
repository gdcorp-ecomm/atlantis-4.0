using System;
using Atlantis.Framework.CDSRepository.Impl.RepositoryHelpers;
using Atlantis.Framework.CDSRepository.Interface;
using Atlantis.Framework.Interface;
using MongoDB.Bson;

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

        //Setup and attemp PRIMARY
        DocumentRepository = GetPrimaryDocumentRepository(config);

        ObjectId docId;
        bool isValidObjectId = ObjectId.TryParse(cdsRequestData.DocumentId, out docId);
        bool bypassCache = isValidObjectId || cdsRequestData.ActiveDate > default(DateTime);

        try
        {
          if (bypassCache)
          {
            // allows for non-cached preview type request
            result = DocumentRepository.GetDocument(cdsRequestData.Query, docId, cdsRequestData.ActiveDate);
          }
          else
          {
            result = DocumentRepository.GetDocument(cdsRequestData.Query);
          }
        }
        catch(Exception ex)
        {
          // log that Primary bombed
          string message = ex.Message + Environment.NewLine + ex.StackTrace;
          Engine.Engine.LogAtlantisException(new AtlantisException(cdsRequestData, "CDSRepositoryRequest.RequestHandler", message, cdsRequestData.Query, ex));

          // When Primary bombs, failover to SECONDARY
          DocumentRepository = GetSecondaryDocumentRepository(config);
          result = DocumentRepository.GetDocument(cdsRequestData.Query);
        }

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

    private IDocumentRepository GetPrimaryDocumentRepository(ConfigElement config)
    {
      string connectionString = config.GetConfigValue("MongoDBConnection");
      return new MongoDBDocumentRepository(connectionString);
    }
    private IDocumentRepository GetSecondaryDocumentRepository(ConfigElement config)
    {
      string rootPath = config.GetConfigValue("RootPath");
      return new FlatFileDocumentRepository(rootPath);
    }

  }
}
