using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SplitTesting.Interface;
using System;

namespace Atlantis.Framework.SplitTesting.Impl
{
  public class ActiveSplitTestDetailsRequest : IRequest
  {
    private const string RequestFormat = "<GetSplitTestDetail><param name=\"splitTestID\" value=\"{0}\"/></GetSplitTestDetail>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        var request = ((ActiveSplitTestDetailsRequestData)requestData);
        var requestXml = string.Format(RequestFormat, request.SplitTestId);

        var cacheXml = string.Empty;
        using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          cacheXml = comCache.GetCacheData(requestXml);
          //cacheXml = "<data count=\"1\"><item SplitTestSideID=\"1\" SideName=\"A\" PercentAllocation=\"50.00\"/><item SplitTestSideID=\"1\" SideName=\"B\" PercentAllocation=\"50.00\" /></data>";
        }
        result = ActiveSplitTestDetailsResponseData.FromCacheXml(cacheXml);
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException(requestData, "ActiveSplitTestDetailsRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = ActiveSplitTestDetailsResponseData.FromException(exception);
      }

      return result;
    }
  }
}
