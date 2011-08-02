using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.KiwiLogger.Interface
{
  public class KiwiLoggerResponseData : IResponseData
  {
    string m_sResponseXML;
    AtlantisException m_ex;

    public KiwiLoggerResponseData(string sResponseXML)
    {
      m_sResponseXML = sResponseXML;
      m_ex = null;
    }

    // **************************************************************** //

    public KiwiLoggerResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      m_sResponseXML = sResponseXML;
      m_ex = exAtlantis;
    }

    // **************************************************************** //

    public KiwiLoggerResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      m_sResponseXML = sResponseXML;
      m_ex = new AtlantisException(oRequestData, "KiwiLoggerResponseData", ex.Message, string.Empty);
    }

    // **************************************************************** //

    public bool IsSuccess
    {
      get { return m_sResponseXML.IndexOf("success", StringComparison.OrdinalIgnoreCase) > -1; }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return m_sResponseXML;
    }

    public AtlantisException GetException()
    {
      return m_ex;
    }

    #endregion
  }
}
