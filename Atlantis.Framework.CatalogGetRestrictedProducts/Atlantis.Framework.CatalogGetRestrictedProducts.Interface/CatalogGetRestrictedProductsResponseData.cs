using System;
using System.Collections.Generic;
using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CatalogGetRestrictedProducts.Interface
{
  public class CatalogGetRestrictedProductsResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public DataTable ResultData { get; private set; }
    public Dictionary<string, HashSet<int>> RestrictedProductsByMgrUserId { get; private set; }

    public CatalogGetRestrictedProductsResponseData(RequestData requestData, DataTable resultData)
    {
      _exception = null;
      ResultData = resultData;
      CatalogGetRestrictedProductsRequestData request = requestData as CatalogGetRestrictedProductsRequestData;
      RestrictedProductsByMgrUserId = new Dictionary<string, HashSet<int>>();

      if ((ResultData != null) && (ResultData.Rows.Count > 0))
      {
        foreach (DataRow dr in ResultData.Rows)
        {
          if (IsColumnNotNull("RestrictedProductMgr", dr))
          {
            string mgrId = Convert.ToString(dr["RestrictedProductMgr"]);
            if (!RestrictedProductsByMgrUserId.ContainsKey(mgrId))
            {
              RestrictedProductsByMgrUserId.Add(mgrId, new HashSet<int>());
            }
            if (IsColumnNotNull("pf_id", dr))
            {
              int pf_id = Convert.ToInt32(dr["pf_id"]);

              if (!RestrictedProductsByMgrUserId[mgrId].Contains(pf_id))
              {
                RestrictedProductsByMgrUserId[mgrId].Add(pf_id);
              }
            }
          }
        }
      }
      _isSuccess = true;
    }

    public CatalogGetRestrictedProductsResponseData(RequestData requestData, Exception exception)
    {
      string privateLabelId = string.Empty;
      CatalogGetRestrictedProductsRequestData request = requestData as CatalogGetRestrictedProductsRequestData;

      if (request != null)
      {
        privateLabelId = request.PrivateLabelId;
      }

      string data = string.Concat("shopper id=", requestData.ShopperID, ";privatelabelid=", privateLabelId);
      string description = exception.Message;
      _exception
        = new AtlantisException("CatalogGetRestrictedProductsRequest", requestData.SourceURL, "0", description, data,
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
