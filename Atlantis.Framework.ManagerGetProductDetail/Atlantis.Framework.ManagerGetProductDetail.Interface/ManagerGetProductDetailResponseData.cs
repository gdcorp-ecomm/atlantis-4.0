using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Data;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.ManagerGetProductDetail.Interface
{
  public class ManagerGetProductDetailResponseData: IResponseData
  {
    private bool _success = false;
    private AtlantisException _atlantisEx = null;

    public DataTable ProductCatalogDetails { get; private set; }

    public bool IsSuccess
    { 
      get { return _success; } 
    }

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
      string message = ex.ToString();
      string data = string.Format("sid={0}", oRequestData.ShopperID);
      _atlantisEx = new AtlantisException(oRequestData, "ManagerGetProductDetailResponseData", message, data, ex);
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _atlantisEx;
    }

    public string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

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

  }
}
