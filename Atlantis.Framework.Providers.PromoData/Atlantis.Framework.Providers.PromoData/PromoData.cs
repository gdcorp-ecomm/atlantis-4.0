using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoData.Interface;
using Atlantis.Framework.Providers.Interface.PromoData;

namespace Atlantis.Framework.Providers.PromoData
{
  class PromoData : IPromoData
  {
    public PromoData(string promoCode)
    {
      this._promoCode = promoCode;

      try
      {
        GetPromoData(promoCode);
      }
      catch (Exception ex)
      {
        LogException("Atlantis.Framework.Providers.PromoData", ex.Message, ex.Source);
      }
    }

    #region Properties

    private IPromoProduct _promoProduct;

    private string _promoCode = string.Empty;
    public string PromoCode
    {
      get { return this._promoProduct.PromoCode; }
    }

    public bool IsPromoActive
    {
      get { return this._promoProduct.IsPromoActive; }
    }

    private string _disclaimer = string.Empty;
    public string Disclaimer
    {
      get { return this._promoProduct.Disclaimer; }
    }

    private DateTime _promoStartDate = DateTime.MinValue;
    public DateTime PromoStartDate
    {
      get { return this._promoProduct.PromoStartDate; }
    }

    private DateTime _promoEndDate = DateTime.MinValue;
    public DateTime PromoEndDate
    {
      get { return this._promoProduct.PromoEndDate; }
    }

    #endregion Properties

    #region Public Methods

    public bool IsActivePromoForPrivateLabelTypeId(int privateLabelTypeId)
    {
      if (this._promoProduct == null)
      {
        return false;
      }
      else
      {
        return this._promoProduct.IsActivePromoForPrivateLabelTypeId(privateLabelTypeId);
      }
    }

    public int GetAwardAmount(int productId, string currencyType, out string awardType)
    {
      IProductAward award = this._promoProduct.GetProductAward(productId);
      awardType = award.AwardType;
      int amount = award.GetAwardAmount(currencyType);
      return amount;
    }

    #endregion Public Methods

    #region Private Methods

    private void GetPromoData(string promoCode)
    {
      PromoDataRequestData request = new PromoDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, promoCode);
      int timeout;
      string appSetting = DataCache.DataCache.GetAppSetting("ATLANTIS_PROMODATA_TIMEOUT");

      if (!int.TryParse(appSetting, out timeout))
      {
        timeout = 5000;
      }
      else if (timeout < 5000)
      {
        timeout = 5000;
      }

      request.RequestTimeout = TimeSpan.FromMilliseconds(timeout);
      PromoDataResponseData response
        = (PromoDataResponseData)DataCache.DataCache.GetProcessRequest(request, PromoDataEngineRequests.GetPromoDataRequest);
      this._promoProduct = response.PromoProduct;
      this._promoEndDate = response.PromoProduct.PromoEndDate;
      this._promoStartDate = response.PromoProduct.PromoStartDate;
      this._disclaimer = response.PromoProduct.Disclaimer;
    }

    private void LogException(string sourceFunction, string message, string source)
    {
      AtlantisException aex = new AtlantisException(sourceFunction, "0", message, source, null, null);
      Engine.Engine.LogAtlantisException(aex);
    }

    #endregion Private Methods
  }
}
