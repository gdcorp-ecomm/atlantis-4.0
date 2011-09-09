using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CostcoRemoveMemberInfo.Interface
{
  public class CostcoRemoveMemberInfoResponseData : IResponseData
  {
    #region Constructors

    public CostcoRemoveMemberInfoResponseData(string response, bool bIsSuccess, string msgForUser)
    {
      IsSuccess = bIsSuccess;
      Response = response;      
      MessageForUser = !bIsSuccess && String.IsNullOrEmpty(msgForUser) ? kMessageToUserOnException : msgForUser;
    }

    public CostcoRemoveMemberInfoResponseData(CostcoRemoveMemberInfoRequestData requestData, Exception ex)
    {
      string data = String.Concat("requestData.RequestTimeout(ms)=", requestData.RequestTimeout.Milliseconds);
      InitWithException(requestData, ex, data);
    }

    public CostcoRemoveMemberInfoResponseData(RequestData requestData, Exception ex)
    {
      InitWithException(requestData, ex, null);
    }

    private void InitWithException(RequestData requestData, Exception ex, string data)
    {
      IsSuccess = false;
      MessageForUser = kMessageToUserOnException;
      _exception = BuildException(requestData, "Constructor", ex, data);
    }

    #endregion 

    #region Helpers

    private static readonly string kMessageToUserOnException = "An error occurred. If it persists, please contact Customer Support.";

    private static AtlantisException BuildException(RequestData requestData, string sourceFunction, Exception ex, string data)
    {
      return new AtlantisException(requestData, "CostcoREmoveMemberInfoResponseData." + sourceFunction, ex.Message + Environment.NewLine + ex.StackTrace, data, ex);
    }

    #endregion

    public bool IsSuccess
    {
      get;
      private set;
    }

    public string Response
    {
      get;
      private set;
    }
    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    private AtlantisException _exception;
    public AtlantisException GetException()
    {
      return _exception;
    }

    public string MessageForUser
    {
      get;
      private set;
    }

    #endregion
  }
}
