using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackagerProductGroup.Interface
{
  public class ProductPackagerProductGroupResponseData : IResponseData
  {
    private readonly AtlantisException _atlException;

    public IList<IProductGroup> ProductGroupData { get; private set; } 

    public ProductPackagerProductGroupResponseData(IList<IProductGroup> productGroupData)
    {
      ProductGroupData = productGroupData;
    }

    public ProductPackagerProductGroupResponseData(RequestData requestData, Exception ex)
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
