using System;
using System.IO;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorStatus.Interface
{
  public class AuthTwoFactorStatusResponseData : IResponseData
  {

    private string m_sResponseXML;
    AtlantisException m_ex;
    private bool _isSuccess = false;
    private bool _isOff = false;
    private bool _isPending = false;
    private bool _isEnabled = false;

    public AuthTwoFactorStatusResponseData(string twoFactorStatusXML)
    {
      _isSuccess = true;
      ProcessStatus(twoFactorStatusXML);
      m_sResponseXML = twoFactorStatusXML;
      m_ex = null;
    }

    public AuthTwoFactorStatusResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      m_sResponseXML = sResponseXML;
      m_ex = exAtlantis;
    }
    
    public AuthTwoFactorStatusResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      m_sResponseXML = sResponseXML;
      m_ex = new AtlantisException(oRequestData, 
                                   "AuthTwoFactorStatus", 
                                   ex.Message, string.Empty);
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public bool IsPending
    {
      get { return _isPending; }
    }

    public bool IsEnabled
    {
      get { return _isEnabled; }
    }

    public bool IsOff
    {
      get { return _isOff; }
    }


    private void ProcessStatus(string xml)
    {
      string code = string.Empty;
      using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
      {
        while (reader.Read())
        {
          if (string.Equals(reader.Name, "Shopper"))
          {
            code = reader["TwoFactorStatus"];
          }
        }
      }

      switch (code.ToLowerInvariant())
      {
        case "off":
          _isOff = true;
          break;
        case "pending":
          _isPending = true;
          break;
        case "enabled":
          _isEnabled = true;
          break;
      }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return m_ex;
    }

    public string ToXML()
    {
      return m_sResponseXML;
    }

    #endregion

  }

}
