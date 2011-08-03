using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetDurationHash.Interface
{
  public class GetDurationHashRequestData : RequestData
  {
    public int UnifiedPFID { get; set; }
    public int PrivateLabelID { get; set; }
    public double Duration { get; set; }
    
    public GetDurationHashRequestData(string shopperId,
                                      string sourceUrl,
                                      string orderId,
                                      string pathway,
                                      int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      UnifiedPFID = 0;
      PrivateLabelID = 0;
      Duration = 0;
    }

    // **************************************************************** //

    public GetDurationHashRequestData(string shopperId,
                                      string sourceUrl,
                                      string orderId,
                                      string pathway,
                                      int pageCount,
                                      int unifiedPfid,
                                      int privateLabelId,
                                      double dDuration)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      UnifiedPFID = unifiedPfid;
      PrivateLabelID = privateLabelId;
      Duration = dDuration;
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("GetDurationHash is not a cacheable request.");
    }

    #endregion

  }
}
