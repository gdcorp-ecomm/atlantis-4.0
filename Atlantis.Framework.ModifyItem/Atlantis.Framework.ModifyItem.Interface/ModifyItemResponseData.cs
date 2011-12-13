using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ModifyItem.Interface
{
  public class ModifyItemResponseData : IResponseData
  {
    // **************************************************************** //

    string m_sResponseXML;
    AtlantisException m_ex;

    // **************************************************************** //

    public ModifyItemResponseData(string sResponseXML)
    {
      m_sResponseXML = sResponseXML;
      m_ex = null;
    }

    // **************************************************************** //

    public ModifyItemResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      m_sResponseXML = sResponseXML;
      m_ex = exAtlantis;
    }

    // **************************************************************** //

    public ModifyItemResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      m_sResponseXML = sResponseXML;
      m_ex = new AtlantisException(oRequestData, "ModifyItemResponseData", ex.Message, oRequestData.ToXML());
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
