using System;
using System.Collections.Generic;
using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaAvailableProductNamespaces.Interface
{
  public class MyaAvailableProductNamespacesResponseData: IResponseData
  {
    private readonly AtlantisException _exception;

    public MyaAvailableProductNamespacesResponseData(DataTable data, string culture)
    {
      ProductNamespaces = GetProductNamespaces(data, culture);
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

    static IEnumerable<ProductNamespace> GetProductNamespaces(DataTable data, string culture)
    {
      var productNamespaces = new List<ProductNamespace>();
      if (data.Rows != null)
      {
        FetchResource resourceFetcher = new FetchResource("Atlantis.Framework.MyaAvailableProductNamespaces.Interface.LanguageResources.MyaAvailableProductNamespaces", culture);

        foreach (DataRow row in data.Rows)
        {
          var name = Convert.ToString(row["namespace"]);
          var sortOrder = Convert.ToString(row["sortOrder"]);
          var productGroup = Convert.ToString(row["pl_productGroupID"]);
          
          string languageKey = string.Concat("myaNs", name.ToLower());
          string keyValue = resourceFetcher.GetString(languageKey);

          string description;
          string example;
          string note;
          GetValues(keyValue, out description, out example, out note);
          
          productNamespaces.Add(new ProductNamespace(name, description, example, note, sortOrder, productGroup));
        }
      }

      return productNamespaces.AsReadOnly();
    }

    private static void GetValues(string keyValues, out string description, out string example, out string note)
    {
      description = string.Empty;
      example = string.Empty;
      note = string.Empty;

      string[] keyArray = keyValues.Split('|');
      int i = 0;

      foreach(string value in keyArray)
      {
        switch (i)
        {
          case 0:
            description = value;
            break;
          case 1:
            example = value;
            break;
          case 2:
            note = value;
            break;
        }

        i++;
      }
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
