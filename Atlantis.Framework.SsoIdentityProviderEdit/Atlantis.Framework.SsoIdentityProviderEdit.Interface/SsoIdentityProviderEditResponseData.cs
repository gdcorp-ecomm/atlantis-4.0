using System;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoIdentityProviderEdit.Interface
{
  public class SsoIdentityProviderEditResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;

    public bool IsSuccess { get; private set; }

    public SsoIdentityProviderEditResponseData() 
    {
      IsSuccess = true;
    }

    public SsoIdentityProviderEditResponseData(AtlantisException atlantisException)
    {
      IsSuccess = false;
      _atlantisException = atlantisException;
    }

    public SsoIdentityProviderEditResponseData(RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      _atlantisException = new AtlantisException(requestData
        , MethodBase.GetCurrentMethod().DeclaringType.FullName
        , string.Format("SsoIdentityProviderEditResponseData Error: {0}", ex.Message)
        , ex.Data.ToString()
        , ex);                                   
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in SsoIdentityProviderEditResponseData");
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion IResponseData Members
  }
}