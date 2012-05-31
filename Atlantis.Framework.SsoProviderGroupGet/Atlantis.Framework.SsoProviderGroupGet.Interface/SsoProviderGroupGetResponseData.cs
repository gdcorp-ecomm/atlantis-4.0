using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoProviderGroupGet.Interface
{
  public class SsoProviderGroupGetResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;
    private readonly List<ServiceProviderGroupItem> _serviceProviderGroups;

    public bool IsSuccess { get; private set; }

    public List<ServiceProviderGroupItem> ServiceProviderGroups
    {
      get { return _serviceProviderGroups; }
    }

    public SsoProviderGroupGetResponseData(List<ServiceProviderGroupItem> serviceProviderGroups) 
    {
      _serviceProviderGroups = serviceProviderGroups;
      IsSuccess = true;
    }

    public SsoProviderGroupGetResponseData(AtlantisException atlantisException)
    {
      IsSuccess = false;
      _atlantisException = atlantisException;
    }

    public SsoProviderGroupGetResponseData(RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      _atlantisException = new AtlantisException(requestData
        , MethodBase.GetCurrentMethod().DeclaringType.FullName
        , string.Format("SsoProviderGroupGetResponseData Error: {0}", ex.Message)
        , ex.Data.ToString()
        , ex);                                   
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in SsoProviderGroupGetResponseData");
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion IResponseData Members

  }
}
