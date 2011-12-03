using System;
using System.Collections.Generic;
using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CatalogGetCategoriesCache.Interface
{
  public class CatalogGetCategoriesCacheResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public DataTable ResultData { get; private set; }
    public HashSet<int> AdminOnlyProducts { get; private set; }
    public HashSet<int> RestrictedProducts { get; private set; }
    public HashSet<int> AllProducts { get; private set; }

    public CatalogGetCategoriesCacheResponseData(RequestData requestData, DataTable resultData)
    {
      _exception = null;
      ResultData = resultData;
      CatalogGetCategoriesCacheRequestData request = requestData as CatalogGetCategoriesCacheRequestData;
      AdminOnlyProducts = new HashSet<int>();
      RestrictedProducts = new HashSet<int>();
      AllProducts = new HashSet<int>();

      if ((ResultData != null) && (ResultData.Rows.Count > 0))
      {
        foreach (DataRow dr in ResultData.Rows)
        {
          if (IsColumnNotNull("pf_id", dr) && IsColumnNotNull("AdminOnly", dr) && IsColumnNotNull("RestrictedProduct", dr))
          {
            int pf_id = Convert.ToInt32(dr["pf_id"]);            
            bool adminOnly = Convert.ToBoolean(dr["AdminOnly"]);
            bool restrictedProduct = Convert.ToBoolean(dr["RestrictedProduct"]);

            if (adminOnly && !AdminOnlyProducts.Contains(pf_id))
            {
              AdminOnlyProducts.Add(pf_id);
            }
            if (restrictedProduct && !RestrictedProducts.Contains(pf_id))
            {
              RestrictedProducts.Add(pf_id);
            }
            if (!AllProducts.Contains(pf_id))
            {
              AllProducts.Add(pf_id);
            }
          }
        }
      }
      _isSuccess = true;
    }

    public CatalogGetCategoriesCacheResponseData(RequestData requestData, Exception exception)
    {
      string privateLabelId = string.Empty;
      string versionNumber = string.Empty;
      CatalogGetCategoriesCacheRequestData request = requestData as CatalogGetCategoriesCacheRequestData;

      if (request != null)
      {
        privateLabelId = request.PrivateLabelId;
        versionNumber = request.VersionNumber.ToString();
      }

      string data = string.Concat("shopper id=", requestData.ShopperID, ";privatelabelid=", privateLabelId, ";versionNumber=", versionNumber);
      string description = exception.Message;
      _exception
        = new AtlantisException("CatalogGetCategoriesCacheRequest", requestData.SourceURL, "0", description, data,
        requestData.ShopperID, requestData.OrderID, string.Empty,
        requestData.Pathway, requestData.PageCount);
      _isSuccess = false;
    }

    private bool _isSuccess;
    public bool IsSuccess
    {
      get
      {
        return _isSuccess;
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      System.IO.StringWriter writer = new System.IO.StringWriter();
      ResultData.WriteXml(writer, XmlWriteMode.IgnoreSchema, false);

      return writer.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    private bool IsColumnNotNull(string columnName, DataRow row)
    {
      return row[columnName] != null && !(row[columnName] is DBNull);
    }

    #endregion
  }
}
