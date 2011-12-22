using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYAWstQuoteStatusByErid.Impl.QuoteWebService;
using Atlantis.Framework.MYAWstQuoteStatusByErid.Interface;

namespace Atlantis.Framework.MYAWstQuoteStatusByErid.Impl
{
  public class MYAWstQuoteStatusByEridRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MYAWstQuoteStatusByEridResponseData responseData = null;

      try
      {
        MYAWstQuoteStatusByEridRequestData quoteStatusRequestData = (MYAWstQuoteStatusByEridRequestData)requestData;
        QuoteWebService.MYAService quoteWS = new MYAService();
        quoteWS.Url = (((WsConfigElement)config).WSURL);
        quoteWS.Timeout = (int)quoteStatusRequestData.RequestTimeout.TotalMilliseconds;

        string responseXml = quoteWS.GetStatusByAccountId(quoteStatusRequestData.ExternalResourceId);

        responseData = new MYAWstQuoteStatusByEridResponseData(responseXml);
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new MYAWstQuoteStatusByEridResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new MYAWstQuoteStatusByEridResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
