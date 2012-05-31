using System;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoServiceProviderEdit.Interface
{
  public class SsoServiceProviderEditResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;

    public bool IsSuccess { get; private set; }

    public SsoServiceProviderEditResponseData() 
    {
      IsSuccess = true;
    }

    public SsoServiceProviderEditResponseData(AtlantisException atlantisException)
    {
      IsSuccess = false;
      _atlantisException = atlantisException;
    }

    public SsoServiceProviderEditResponseData(RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      _atlantisException = new AtlantisException(requestData
        , MethodBase.GetCurrentMethod().DeclaringType.FullName
        , string.Format("SsoServiceProviderEditResponseData Error: {0}", ex.Message)
        , ex.Data.ToString()
        , ex);                                   
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in SsoServiceProviderEditResponseData");
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion IResponseData Members

  }
}