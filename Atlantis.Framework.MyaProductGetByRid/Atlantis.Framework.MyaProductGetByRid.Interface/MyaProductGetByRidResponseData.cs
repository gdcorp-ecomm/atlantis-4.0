using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaProductGetByRid.Interface
{
  public class MyaProductGetByRidResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;
    public MyaProductAccount ProductAccount { get; set; }

    public bool IsSuccess
    {
      get { return _atlantisException == null; }
    }

    public MyaProductGetByRidResponseData(MyaProductAccount product)
    {
      ProductAccount = product;
    }

    public MyaProductGetByRidResponseData(AtlantisException atlantisException)
    {
      _atlantisException = atlantisException;
      ProductAccount = null;
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
