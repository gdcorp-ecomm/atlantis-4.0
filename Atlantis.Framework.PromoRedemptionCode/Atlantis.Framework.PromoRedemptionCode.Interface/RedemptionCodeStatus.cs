using System;

namespace Atlantis.Framework.PromoRedemptionCode.Interface
{
  public class RedemptionCodeStatus
  {
    public RedemptionCodeStatus(int redemptionCodeId,
                                int redemptionCodeStatusId,
                                string redemptionCodeStatusDescription,
                                DateTime actionDate,
                                DateTime createDate,
                                int packageId,
                                string packageDescription,
                                string promoTrackingCode,
                                string partnerDescription)
    {
      RedemptionCodeId = redemptionCodeId;
      RedemptionCodeStatusId = redemptionCodeStatusId;
      RedemptionCodeStatusDescription = redemptionCodeStatusDescription;
      ActionDate = actionDate;
      CreateDate = createDate;
      PackageId = packageId;
      PackageDescription = packageDescription;
      PromoTrackingCode = promoTrackingCode;
      PartnerDescription = partnerDescription;
    }

    public int RedemptionCodeId { get; private set; }
    public int RedemptionCodeStatusId { get; private set; }
    public string RedemptionCodeStatusDescription { get; private set; }
    public DateTime ActionDate { get; private set; }
    public DateTime CreateDate { get; private set; }
    public int PackageId { get; private set; }
    public string PackageDescription { get; private set; }
    public string PromoTrackingCode { get; private set; }
    public string PartnerDescription { get; private set; }
  }
}