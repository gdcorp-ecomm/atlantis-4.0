using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.GetSurveyTakenInfo.Interface
{
  public class GetSurveyTakenInfoResponseData : IResponseData, ISessionSerializableResponse
  {
    #region Members

    private AtlantisException _ex;

    #endregion

    #region Properties

    /// <summary>
    /// Xml string with the info about surveys taken.
    /// </summary>
    public string SurveyTakenInfo { get; private set; }

    #endregion

    #region Constructors

    public GetSurveyTakenInfoResponseData()
    { }

    public GetSurveyTakenInfoResponseData(string surveyTakenInfo)
    {
      SurveyTakenInfo = surveyTakenInfo;
    }

    public GetSurveyTakenInfoResponseData(AtlantisException ex)
    {
      _ex = ex;
    }

    public GetSurveyTakenInfoResponseData(RequestData requestData, Exception ex)
    {
      _ex = new AtlantisException(requestData, "GetSurveyTakenInfoResponseData", ex.Message, requestData.ToXML());
    }
    
    #endregion



    #region IResponseData Members

    public string ToXML()
    {
      return SurveyTakenInfo;
    }

    public AtlantisException GetException()
    {
      return _ex;
    }
    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      if (!string.IsNullOrEmpty(sessionData))
      {
        SurveyTakenInfo = sessionData;
      }
    }
    #endregion
  }
}
