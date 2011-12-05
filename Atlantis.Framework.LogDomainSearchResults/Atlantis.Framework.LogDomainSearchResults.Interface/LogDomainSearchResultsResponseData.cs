using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LogDomainSearchResults.Interface
{
  public class LogDomainSearchResultsResponseData : IResponseData
  {
    private bool _success = false;
    private AtlantisException _ex;

    public LogDomainSearchResultsResponseData()
    {
      _success = true;
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public LogDomainSearchResultsResponseData(AtlantisException ex)
    {
      _ex = ex;
    }

    public LogDomainSearchResultsResponseData(RequestData oRequestData, Exception ex)
    {
      _ex = new AtlantisException(oRequestData, "LogDomainSearchResultsResponseData", ex.Message, oRequestData.ToXML());
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
