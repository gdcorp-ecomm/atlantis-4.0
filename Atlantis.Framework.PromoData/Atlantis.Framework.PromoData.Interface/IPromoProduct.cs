using System;
using System.Collections.Generic;

namespace Atlantis.Framework.PromoData.Interface
{
  public interface IPromoProduct
  {
    string PromoCode { get; }
    DateTime PromoStartDate { get; }
    DateTime PromoEndDate { get; }
    bool IsPromoActive { get; }
    HashSet<int> PrivateLabelTypes { get; }
    string Disclaimer { get; }
    List<IProductAward> ProductAwards { get; }
    IProductAward GetProductAward(int productId);
    bool IsActivePromoForPrivateLabelTypeId(int privateLabelTypeId);
  }
}
