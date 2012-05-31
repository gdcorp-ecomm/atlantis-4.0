using System;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoProviderGroupEdit.Interface
{
  public class SsoProviderGroupEditResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;

    public bool IsSuccess { get; private set; }

    public SsoProviderGroupEditResponseData() 
    {
      IsSuccess = true;
    }

    public SsoProviderGroupEditResponseData(AtlantisException atlantisException)
    {
      IsSuccess = false;
      _atlantisException = atlantisException;
    }

    public SsoProviderGroupEditResponseData(RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      _atlantisException = new AtlantisException(requestData
        , MethodBase.GetCurrentMethod().DeclaringType.FullName
        , string.Format("SsoProviderGroupEditResponseData Error: {0}", ex.Message)
        , ex.Data.ToString()
        , ex);                                   
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in SsoProviderGroupEditResponseData");
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion IResponseData Members

  }
}
