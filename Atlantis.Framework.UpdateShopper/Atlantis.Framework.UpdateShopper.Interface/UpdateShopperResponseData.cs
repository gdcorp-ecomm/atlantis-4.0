using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.UpdateShopper.Interface
{
  public class UpdateShopperResponseData : IResponseData
  {
    // **************************************************************** //

    string m_sResponseXML;
    AtlantisException m_ex;

    // **************************************************************** //

    public UpdateShopperResponseData(string sUpdateShopperXML)
    {
      m_sResponseXML = sUpdateShopperXML;
      m_ex = null;
    }
    
    // **************************************************************** //

    public UpdateShopperResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      m_sResponseXML = sResponseXML;
      m_ex = exAtlantis;
    }
    
    // **************************************************************** //

    public UpdateShopperResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      m_sResponseXML = sResponseXML;
      m_ex = new AtlantisException(oRequestData, 
                                   "UpdateShopperResponseData", 
                                   ex.Message, string.Empty);
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
