using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SurveyService.Interface;

namespace Atlantis.Framework.SurveyService.Impl
{
  public class SurveyServiceRequest : IRequest
  {
    // **************************************************************** //

    #region IRequest Members

    // **************************************************************** //

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      SurveyServiceResponseData oResponseData = null;
      string sResponseXML = "";

      try
      {
        SurveyServiceRequestData oSurveyRequestData = (SurveyServiceRequestData)oRequestData;
        WSCgdSurvey.WSCgdSurveyService oSurveyWS = new WSCgdSurvey.WSCgdSurveyService();
        oSurveyWS.Url = ((WsConfigElement)oConfig).WSURL;
        sResponseXML = oSurveyWS.SaveSBAnswers(oSurveyRequestData.IPAddress,
                                               oSurveyRequestData.AdVersion,
                                               oSurveyRequestData.AgeGroupID,
                                               oSurveyRequestData.PoliticalID,
                                               oSurveyRequestData.Answers);

        oResponseData = new SurveyServiceResponseData(sResponseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new SurveyServiceResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new SurveyServiceResponseData(sResponseXML, oRequestData, ex);
      }

      return oResponseData;
    }

    // **************************************************************** //

    #endregion

    // **************************************************************** //
  }
}
