using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GadgetsVote.Interface
{
  public class GadgetsVoteRequestData : RequestData
  {
    public int VoteCode { get; set; }
    
    public GadgetsVoteRequestData(string shopperId, 
                                  string sourceURL, 
                                  string orderId, 
                                  string pathway, 
                                  int pageCount, 
                                  int voteCode,
                                  TimeSpan requestTimeout) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      VoteCode = voteCode;
      RequestTimeout = requestTimeout;
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("GadgetsVote is not a cacheable request.");
    }

    #endregion
  }
}
