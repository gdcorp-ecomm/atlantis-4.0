using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMResellerOptIn.Interface
{
  public class EEMResellerOptInResponseData : IResponseData
  {
    private readonly Exception _exception;

    public bool IsSuccess { get; private set; }

    public EEMResellerOptInResponseData(bool success)
    {
      IsSuccess = success;
    }

    public EEMResellerOptInResponseData(RequestData request, Exception aex)
    {
      IsSuccess = false;
      _exception = new AtlantisException(request, aex.Source, aex.Message, aex.StackTrace);
    }

    public EEMResellerOptInResponseData(Exception ex)
    {
      IsSuccess = false;
      _exception = ex;
    }


    #region Implementation of IResponseData

    public string ToXML()
    {
      const string returnXml = "<EEMResellerOptInResponseData><Success>{0}</Success><EEMResellerOptInResponseData>";
      return string.Format(returnXml, IsSuccess);
    }

    public AtlantisException GetException()
    {
      return (AtlantisException) _exception;
    }

    #endregion
  }
}
