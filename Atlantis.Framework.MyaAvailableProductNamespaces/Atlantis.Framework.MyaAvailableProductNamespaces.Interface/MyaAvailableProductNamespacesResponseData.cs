using System;
using System.Collections.Generic;
using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaAvailableProductNamespaces.Interface
{
  public class MyaAvailableProductNamespacesResponseData: IResponseData
  {
    private readonly AtlantisException _exception;

    public MyaAvailableProductNamespacesResponseData(DataTable data)
    {
      ProductNamespaces = GetProductNamespaces(data);
      IsSuccess = true;
    }

    public MyaAvailableProductNamespacesResponseData(Exception ex, RequestData requestData)
    {
      _exception = new AtlantisException(requestData, "MyaAvailableProductNamespacesResponseData", ex.Message, string.Empty, ex);
    }

    public MyaAvailableProductNamespacesResponseData(AtlantisException ex)
    {
      _exception = ex;
    }

    static IEnumerable<ProductNamespace> GetProductNamespaces(DataTable data)
    {
      var productNamespaces = new List<ProductNamespace>();
      if (data.Rows != null)
      {
        foreach (DataRow row in data.Rows)
        {
          var name = Convert.ToString(row["namespace"]);
          var description = Convert.ToString(row["description"]);
          var example = Convert.ToString(row["example"]);
          var note = Convert.ToString(row["note"]);
          var sortOrder = Convert.ToString(row["sortOrder"]);
          var productGroup = Convert.ToString(row["pl_productGroupID"]);

          productNamespaces.Add(new ProductNamespace(name, description, example, note, sortOrder, productGroup));
        }
      }

      return productNamespaces.AsReadOnly();
    }

    public IEnumerable<ProductNamespace> ProductNamespaces { get; private set; }
    public bool IsSuccess { get; private set; }
    
    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
