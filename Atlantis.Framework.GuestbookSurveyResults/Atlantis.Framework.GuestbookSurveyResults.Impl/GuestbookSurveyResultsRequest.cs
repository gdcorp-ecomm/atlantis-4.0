using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.GuestbookSurveyResults.Interface;
using Atlantis.Framework.GuestbookSurveyResults.Impl.GuestbookWS;
using System.Xml;

namespace Atlantis.Framework.GuestbookSurveyResults.Impl
{
  public class GuestbookSurveyResultsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;
      XmlNode responseNode = null;

      try
      {
        GuestbookSurveyResultsRequestData surveyResultsRequest = (GuestbookSurveyResultsRequestData)oRequestData;

        GuestbookService service = new GuestbookService();
        service.Url = ((WsConfigElement)oConfig).WSURL;
        service.Timeout = (int)surveyResultsRequest.RequestTimeout.TotalMilliseconds;

        responseNode = service.GetSurveyResults(surveyResultsRequest.SurveyId);
        result = new GuestbookSurveyResultsResponseData(responseNode, oRequestData);
      }
      catch (AtlantisException aex)
      {
        result = new GuestbookSurveyResultsResponseData(responseNode, aex);
      }
      catch (Exception ex)
      {
        result = new GuestbookSurveyResultsResponseData(responseNode, oRequestData, ex);
      }

      return result;
    }

    #endregion
  }
}
