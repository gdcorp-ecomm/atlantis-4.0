using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CDSRepository.Interface
{

  public class CDSRepositoryResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private string _responseData = string.Empty;
    private readonly bool _success;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public CDSRepositoryResponseData(string responseData)
    {
      _success = true;
      _responseData = responseData;
    }

    public CDSRepositoryResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public CDSRepositoryResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                         "CDSRepositoryGetResponseData",
                                         exception.Message + exception.StackTrace,
                                         requestData.ToXML());
    }

    public string ResponseData
    {
      get
      {
        return _responseData;
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
