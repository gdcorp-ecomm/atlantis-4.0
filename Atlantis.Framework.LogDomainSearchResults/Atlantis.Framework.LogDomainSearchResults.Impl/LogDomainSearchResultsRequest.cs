using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.LogDomainSearchResults.Impl.LogDomainSearchResultsWS;
using Atlantis.Framework.LogDomainSearchResults.Interface;

namespace Atlantis.Framework.LogDomainSearchResults.Impl
{
  public class LogDomainSearchResultsRequest : IRequest
  {

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      LogDomainSearchResultsResponseData oResponseData = null;

      LogDomainSearchResultsWS.Service service = null;
      try
      {
        LogDomainSearchResultsRequestData request = (LogDomainSearchResultsRequestData)oRequestData;
        service = new Service();
        service.Url = ((WsConfigElement)oConfig).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        service.LogDomainSearchResults(request.ToXML());

        oResponseData = new LogDomainSearchResultsResponseData();
      }
      catch (Exception ex)
      {
        oResponseData = new LogDomainSearchResultsResponseData(oRequestData, ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }

      return oResponseData;
    }

    #endregion
  }
}
