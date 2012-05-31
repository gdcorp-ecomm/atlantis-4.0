using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoServiceProviderGet.Interface
{
  public class SsoServiceProviderGetResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;
    private readonly List<SsoServiceProviderItem> _ssoServiceProviders;

    public bool IsSuccess { get; private set; }

    public List<SsoServiceProviderItem> SsoServiceProviders
    {
      get { return _ssoServiceProviders; }
    }

    public SsoServiceProviderGetResponseData(List<SsoServiceProviderItem> ssoServiceProviders) 
    {
      _ssoServiceProviders = ssoServiceProviders;
      IsSuccess = true;
    }

    public SsoServiceProviderGetResponseData(AtlantisException atlantisException)
    {
      IsSuccess = false;
      _atlantisException = atlantisException;
    }

    public SsoServiceProviderGetResponseData(RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      _atlantisException = new AtlantisException(requestData
        , MethodBase.GetCurrentMethod().DeclaringType.FullName
        , string.Format("SsoServiceProviderGetResponseData Error: {0}", ex.Message)
        , ex.Data.ToString()
        , ex);                                   
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in SsoServiceProviderGetResponseData");
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion IResponseData Members

  }
}
