using System.Collections.Generic;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaResourceReverseQty.Interface
{
  public class MyaResourceReverseQtyResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;
    public List<ResourceReverseQty> ResourceReverseQtyList { get; set; }

    public bool IsSuccess
    {
      get { return _atlantisException == null; }
    }

    public MyaResourceReverseQtyResponseData(List<ResourceReverseQty> list)
    {
      ResourceReverseQtyList = list;
    }

    public MyaResourceReverseQtyResponseData(AtlantisException atlantisException)
    {
      _atlantisException = atlantisException;
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion

  }
}
