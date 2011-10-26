using System;

namespace Atlantis.Framework.Providers.Interface.PromoData
{
  public interface IPromoData
  {
    string PromoCode { get; }
    DateTime PromoStartDate { get; }
    DateTime PromoEndDate { get; }
    bool IsPromoActive { get; }
    string Disclaimer { get; }
    bool IsActivePromoForPrivateLabelTypeId(int privateLabelTypeId);
    int GetAwardAmount(int productId, string currencyType, out string awardType);
  }
}
