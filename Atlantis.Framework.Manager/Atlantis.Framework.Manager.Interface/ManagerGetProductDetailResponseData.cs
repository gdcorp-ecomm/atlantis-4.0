using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Manager.Interface
{
  public class ManagerGetProductDetailResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisEx;
    private readonly bool _success;

    public ManagerGetProductDetailResponseData()
    {
    }

    public ManagerGetProductDetailResponseData(DataTable dt)
    {
      ProductCatalogDetails = dt;
      _success = true;
    }

    public ManagerGetProductDetailResponseData(AtlantisException atlantisEx)
    {
      _atlantisEx = atlantisEx;
    }

    public ManagerGetProductDetailResponseData(RequestData oRequestData, Exception ex)
    {
      var message = ex.ToString();
      var data = string.Format("sid={0}", oRequestData.ShopperID);
      _atlantisEx = new AtlantisException(oRequestData, "ManagerGetProductDetailResponseData", message, data, ex);
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _atlantisEx;
    }

    public string ToXML()
    {
      var sbResult = new StringBuilder();
      var xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

      int rowsReturned = 0;
      if (ProductCatalogDetails != null && IsSuccess)
      {
        rowsReturned = ProductCatalogDetails.Rows.Count;
      }

      xtwRequest.WriteStartElement("response");
      xtwRequest.WriteAttributeString("rowsReturned", rowsReturned.ToString());
      xtwRequest.WriteAttributeString("success", IsSuccess.ToString());
      xtwRequest.WriteEndElement();

      return sbResult.ToString();
    }

    #endregion

    public DataTable ProductCatalogDetails { get; private set; }

    public bool IsSuccess
    {
      get { return _success; }
    }
  }
}