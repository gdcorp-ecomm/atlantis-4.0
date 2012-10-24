using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackagerProductPackage.Interface
{
  public class ProductPackagerProductPackageResponseData : IResponseData
  {
    private readonly AtlantisException _atlException;

    public IDictionary<string, IProductPackageData> ProductPackageData { get; private set; }

    public ProductPackagerProductPackageResponseData(IDictionary<string, IProductPackageData> productPackageData)
    {
      ProductPackageData = productPackageData;
    }

    public ProductPackagerProductPackageResponseData(RequestData requestData, Exception ex)
    {
      _atlException = new AtlantisException(requestData, "Atlantis.Framework.FbProductPackageProductGroupResponseData", ex.Message, ex.StackTrace, ex);
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _atlException;
    }
  }
}
