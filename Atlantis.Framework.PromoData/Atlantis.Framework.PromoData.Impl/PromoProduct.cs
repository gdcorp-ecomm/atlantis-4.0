using System;
using System.Collections.Generic;
using Atlantis.Framework.PromoData.Interface;

namespace Atlantis.Framework.PromoData.Impl
{
  public class PromoProduct : IPromoProduct
  {
    private string _promoCode;
    public string PromoCode
    {
      get { return _promoCode; }
    }

    private DateTime _promoStartDate;
    public DateTime PromoStartDate
    {
      get { return _promoStartDate; }
    }

    private DateTime _promoEndDate;
    public DateTime PromoEndDate
    {
      get { return _promoEndDate; }
    }

    private bool _isPromoActive;
    public bool IsPromoActive
    {
      get { return _isPromoActive; }
    }

    private HashSet<int> _privateLabelTypes;
    public HashSet<int> PrivateLabelTypes
    {
      get { return _privateLabelTypes; }
    }

    private string _disclaimer = string.Empty;
    public string Disclaimer
    {
      get { return _disclaimer; }
      set { _disclaimer = value; }
    }

    private List<IProductAward> _productAwards;
    public List<IProductAward> ProductAwards
    {
      get { return _productAwards; }
    }

    public PromoProduct(string promoCode, DateTime promoStartDate, DateTime promoEndDate, bool isPromoActive)
    {
      this._promoCode = promoCode;
      this._promoStartDate = promoStartDate;
      this._promoEndDate = promoEndDate;
      this._isPromoActive = isPromoActive;
      this._privateLabelTypes = new HashSet<int>();
      this._productAwards = new List<IProductAward>();
    }

    public void AddProductAward(IProductAward productAward)
    {
      this._productAwards.Add(productAward);
    }

    public void AddPrivateLabelType(int privateLabelType)
    {
      this._privateLabelTypes.Add(privateLabelType);
    }

    public IProductAward GetProductAward(int productId)
    {
      IProductAward award
        = this._productAwards.Find((IProductAward aw) => (aw.ProductId.Equals(productId)));

      if (award != null)
      {
        return award;
      }
      else
      {
        return new NoAward(productId);
      }
    }

    public bool IsActivePromoForPrivateLabelTypeId(int privateLabelTypeId)
    {
      if (this._privateLabelTypes.Contains(privateLabelTypeId))
      {
        return this._isPromoActive;
      }
      else
      {
        return false;
      }
    }
  }
}
