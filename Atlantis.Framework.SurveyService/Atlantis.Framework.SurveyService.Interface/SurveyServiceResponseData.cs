using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SurveyService.Interface
{
  public class SurveyServiceResponseData : IResponseData
  {
    // **************************************************************** //

    string m_sResponseXML;
    AtlantisException m_ex;

    // **************************************************************** //

    public SurveyServiceResponseData(string sMiniCartXML)
    {
      m_sResponseXML = sMiniCartXML;
      m_ex = null;
    }

    // **************************************************************** //

    public SurveyServiceResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      m_sResponseXML = sResponseXML;
      m_ex = exAtlantis;
    }

    // **************************************************************** //

    public SurveyServiceResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      m_sResponseXML = sResponseXML;
      m_ex = new AtlantisException(oRequestData,
                                   "SurveyServiceResponseData", 
                                   ex.Message, 
                                   oRequestData.ToXML());
    }

    public bool IsSuccess
    {
      get { return m_sResponseXML.IndexOf("success", StringComparison.OrdinalIgnoreCase) > -1; }
    }

    // **************************************************************** //

    #region IResponseData Members

    // **************************************************************** //

    public AtlantisException GetException()
    {
      return m_ex;
    }

    // **************************************************************** //

    public string ToXML()
    {
      return m_sResponseXML;
    }

    // **************************************************************** //

    #endregion

    // **************************************************************** //
  }
}
