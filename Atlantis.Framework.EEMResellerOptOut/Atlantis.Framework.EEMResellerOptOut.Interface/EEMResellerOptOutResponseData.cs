using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMResellerOptOut.Interface
{
  public class EEMResellerOptOutResponseData : IResponseData 
  {
    private readonly Exception _exception;

    public bool IsSuccess { get; private set; }

    public EEMResellerOptOutResponseData(bool success)
    {
      IsSuccess = success;
    }

    public EEMResellerOptOutResponseData(RequestData request, Exception aex)
    {
      IsSuccess = false;
      _exception = new AtlantisException(request, aex.Source, aex.Message, aex.StackTrace);
    }

    public EEMResellerOptOutResponseData(Exception ex)
    {
      IsSuccess = false;
      _exception = ex;
    }


    #region Implementation of IResponseData

    public string ToXML()
    {
      const string returnXml = "<EEMResellerOptOutResponseData><Success>{0}</Success><EEMResellerOptOutResponseData>";
      return string.Format(returnXml, IsSuccess);
    }

    public AtlantisException GetException()
    {
      return (AtlantisException) _exception;
    }

    #endregion
  }
}
