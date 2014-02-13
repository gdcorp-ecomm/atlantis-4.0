using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LogDomainSearchResults.Interface
{
  public class LogDomainSearchResultsResponseData : IResponseData
  {
    private readonly AtlantisException _ex;
    public bool IsSuccess { get; private set; }

    public LogDomainSearchResultsResponseData()
    {
      IsSuccess = true;
    }

    public LogDomainSearchResultsResponseData(RequestData oRequestData, Exception ex)
    {
      _ex = new AtlantisException("LogDomainSearchResultsResponseData", 0, ex.Message, oRequestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }
}