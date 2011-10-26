using System;
using Atlantis.Framework.Providers.Interface.PromoData;

namespace Atlantis.Framework.Providers.PromoData
{
  public class InvalidPromoData : IPromoData
  {
    public string PromoCode { get { return string.Empty; } }
    public DateTime PromoStartDate { get { return DateTime.MinValue; } }
    public DateTime PromoEndDate { get { return DateTime.MinValue; } }
    public bool IsPromoActive { get { return false; } }
    public string Disclaimer { get { return string.Empty; } }

    public bool IsActivePromoForPrivateLabelTypeId(int privateLabelTypeId)
    {
      return false;
    }

    public int GetAwardAmount(int productId, string currencyType, out string awardType)
    {
      awardType = string.Empty;
      return 0;
    }
  }
}
