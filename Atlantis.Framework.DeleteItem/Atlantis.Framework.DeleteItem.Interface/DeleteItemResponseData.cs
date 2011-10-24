using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DeleteItem.Interface
{
  public class DeleteItemResponseData : IResponseData
  {
    // **************************************************************** //

    string m_sResponseXML;
    AtlantisException m_ex;

    // **************************************************************** //

    public DeleteItemResponseData(string sResponseXML)
    {
      m_sResponseXML = sResponseXML;
      m_ex = null;
    }

    // **************************************************************** //

    public DeleteItemResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      m_sResponseXML = sResponseXML;
      m_ex = exAtlantis;
    }

    // **************************************************************** //

    public DeleteItemResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      m_sResponseXML = sResponseXML;
      m_ex = new AtlantisException(oRequestData, "DeleteItemResponseData", ex.Message, oRequestData.ToXML());
    }
    
    // **************************************************************** //

    public bool IsSuccess
    {
      get{ return m_sResponseXML.IndexOf("success", StringComparison.OrdinalIgnoreCase) > -1; }
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
