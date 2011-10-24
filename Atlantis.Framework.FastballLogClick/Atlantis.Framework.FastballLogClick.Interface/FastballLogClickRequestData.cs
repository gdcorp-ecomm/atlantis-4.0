using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballLogClick.Interface
{
  public class FastballLogClickRequestData : RequestData
  {
    private int _privateLabelId = 0;

    public FastballLogClickRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      int privateLabelId, short applicationId, int fbOfferId, string visitGuid, IManagerContext managerContext)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ShopperID = shopperId;
      RequestTimeout = TimeSpan.FromSeconds(6);
      _privateLabelId = privateLabelId;
      ApplicationId = applicationId;      
      FBOfferId = fbOfferId;
      VisitGuid = visitGuid;
      if (managerContext != null)
      {
        if (managerContext.IsManager)
        {
          RepId = managerContext.ManagerUserId;
        }
      }
    }
    
    public string RepId { get; set; }

    public int FBOfferId { get; set; }

    public short ApplicationId { get; set; }

    public string VisitGuid { get; set; }
    
    public override string GetCacheMD5()  
    {
      throw  new NotImplementedException();
    }
  }
}
