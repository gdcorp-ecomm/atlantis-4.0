using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Manager.Interface
{
  public class ManagerGetProductDetailRequestData : RequestData
  {
    public ManagerGetProductDetailRequestData(int nonUnifiedPfid, int privateLabelId, int adminFlag, int managerUserId)
    {
      NonUnifiedPfid = nonUnifiedPfid;
      PrivateLabelId = privateLabelId;
      AdminFlag = adminFlag;
      ManagerUserId = managerUserId;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public decimal NonUnifiedPfid { get; private set; }
    public int PrivateLabelId { get; private set; }
    public int AdminFlag { get; private set; }
    public int ManagerUserId { get; private set; }
  }
}
