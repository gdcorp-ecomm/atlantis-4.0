using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoRedemptionCode.Interface
{
  public class PromoRedemptionCodeRequestData : RequestData
  {
    public PromoRedemptionCodeRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  int vendorId,
                                  string externalRedemptionCode)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      VendorId = vendorId;
      ExternalRedemptionCode = externalRedemptionCode;
    }

    public int VendorId { get; private set; }
    public string ExternalRedemptionCode { get; private set; }

    #region Overridden Methods

    public override string ToXML()
    {
      return string.Empty;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}