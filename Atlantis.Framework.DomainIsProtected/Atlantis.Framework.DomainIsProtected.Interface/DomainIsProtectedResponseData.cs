using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainIsProtected.Interface
{
  public class DomainIsProtectedResponseData : IResponseData
  {
    private AtlantisException _atlException = null;

    public DomainIsProtectedResponseData(bool isDomainProtected)
    {
      IsSuccess = true;
      IsDomainProtected = isDomainProtected;
    }

    public DomainIsProtectedResponseData(AtlantisException exAtlantis)
    {
      IsSuccess = false;
      _atlException = exAtlantis;
    }

    public DomainIsProtectedResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      _atlException = new AtlantisException(oRequestData, "DomainIsProtectedResponseData", ex.Message, string.Empty);
    }

    public bool IsSuccess { get; private set; }
    public bool IsDomainProtected { get; set; }
  
    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _atlException;
    }

    public string ToXML()
    {
      return string.Empty;
    }

    #endregion

  }
}
