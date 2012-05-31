using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoIdentityProviderGet.Interface
{
  public class SsoIdentityProviderGetResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;
    private readonly List<SsoIdentityProviderItem> _ssoIdentityProviders;

    public bool IsSuccess { get; private set; }

    public List<SsoIdentityProviderItem> SsoIdentityProviders
    {
      get { return _ssoIdentityProviders; }
    }

    public SsoIdentityProviderGetResponseData(List<SsoIdentityProviderItem> ssoIdentityProviders) 
    {
      _ssoIdentityProviders = ssoIdentityProviders;
      IsSuccess = true;
    }

    public SsoIdentityProviderGetResponseData(AtlantisException atlantisException)
    {
      IsSuccess = false;
      _atlantisException = atlantisException;
    }

    public SsoIdentityProviderGetResponseData(RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      _atlantisException = new AtlantisException(requestData
        , MethodBase.GetCurrentMethod().DeclaringType.FullName
        , string.Format("SsoIdentityProviderGetResponseData Error: {0}", ex.Message)
        , ex.Data.ToString()
        , ex);                                   
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in SsoIdentityProviderGetResponseData");
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion IResponseData Members

  }
}