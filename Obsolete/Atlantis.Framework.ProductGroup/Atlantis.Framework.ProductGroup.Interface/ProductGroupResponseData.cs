using System;
using System.Data;
using System.IO;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductGroup.Interface
{
  public class ProductGroupResponseData : IResponseData
  {

    DataTable m_dtResult;
    AtlantisException m_ex;

    // **************************************************************** //

    public ProductGroupResponseData(DataTable dtProductGroupData)
    {
      m_dtResult = dtProductGroupData;
      m_ex = null;
    }

    // **************************************************************** //

    public ProductGroupResponseData(DataTable dtResult, AtlantisException exAtlantis)
    {
      m_dtResult = dtResult;
      m_ex = exAtlantis;
    }

    // **************************************************************** //

    public ProductGroupResponseData(DataTable dtResult, RequestData oRequestData, Exception ex)
    {
      m_dtResult = dtResult;
      m_ex = new AtlantisException(oRequestData,
                                   "ProductGroupResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

    // **************************************************************** //

    public DataTable ProductGroupData
    {
      get { return m_dtResult; }
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
      StringBuilder sbResult = new StringBuilder();

      if (m_dtResult != null)
      {
        DataSet dsResult = new DataSet("ProductGroups");
        dsResult.Tables.Add(m_dtResult);
        m_dtResult.TableName = "ProductGroup";
        dsResult.WriteXml(new StringWriter(sbResult));
        dsResult.Tables.Remove(m_dtResult);
      }

      return sbResult.ToString(); ;
    }

    // **************************************************************** //

    #endregion

    // **************************************************************** //
  }
}
