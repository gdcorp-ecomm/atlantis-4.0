using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GadgetsVote.Interface
{
  public class GadgetsVoteResponseData : IResponseData
  {
    public string ResponseString;

    public bool HasError { get; private set; }

    public string ErrorMessage { get; private set; }

    public Dictionary<string, string> VotingResults;

    private AtlantisException _atlEx;
    
    public GadgetsVoteResponseData(string responseString, Dictionary<string, string> votingResults)
    {
      ResponseString = responseString;
      VotingResults = votingResults;

      if (VotingResults.ContainsKey("Error"))
      {
        HasError = true;
        ErrorMessage = VotingResults["Error"];
      }
    }

    public GadgetsVoteResponseData(RequestData requestData, Exception ex)
    {
      ResponseString = string.Empty;
      VotingResults = new Dictionary<string, string>(0);
      _atlEx = new AtlantisException(requestData, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace, ex);
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      return ResponseString;
    }

    public AtlantisException GetException()
    {
      return _atlEx;
    }

    #endregion
  }
}
