using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
namespace Atlantis.Framework.UpdateItem.Interface
{
    public class UpdateItemResponseData : IResponseData
    {
        // **************************************************************** //

        string m_sResponseXML;
        AtlantisException m_ex;

        // **************************************************************** //

        public UpdateItemResponseData(string sResponseXML)
        {
            m_sResponseXML = sResponseXML;
            m_ex = null;
        }

        // **************************************************************** //

        public UpdateItemResponseData(string sResponseXML, AtlantisException exAtlantis)
        {
            m_sResponseXML = sResponseXML;
            m_ex = exAtlantis;
        }

        // **************************************************************** //

        public UpdateItemResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
        {
            m_sResponseXML = sResponseXML;
            m_ex = new AtlantisException(oRequestData, "UpdateItemResponseData", ex.Message, oRequestData.ToXML());
        }

        // **************************************************************** //

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
