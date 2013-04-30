using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SplitTesting.Interface;
using System;

namespace Atlantis.Framework.SplitTesting.Impl
{
  public class ActiveSplitTestsRequest : IRequest
  {
    private const string RequestFormat = "<GetActiveSplitTests><param name=\"categoryName\" value=\"{0}\"/></GetActiveSplitTests>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        var request = ((ActiveSplitTestsRequestData)requestData);
        var requestXml = string.Format(RequestFormat, request.CategoryName);

        var cacheXml = string.Empty;
        using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          //cacheXml = comCache.GetCacheData(requestXml);
          cacheXml = "<data count=\"1\"><item SplitTestID=\"1\" VersionNumber=\"1\" EligibilityRules=\"dataCenter(AP)\" SplitTestRunID=\"1\" TestStartDate=\"04/22/2013\"/></data>";
        }
        result = ActiveSplitTestsResponseData.FromCacheXml(cacheXml);
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException(requestData, "ActiveSplitTestsRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = ActiveSplitTestsResponseData.FromException(exception);
      }

      return result;
    }
  }
}
