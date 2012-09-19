using System;
using System.Collections.Generic;
using System.Configuration;

namespace Atlantis.Framework.PromoOrderLevelCreate.Interface
{
  public class PrivateLabelPromoCurrency
  {
    public enum AwardType
    {
      Unspecified = 0,
      PercentOff = 1,
      DollarOff = 2
    }

    //Failsafe in case we cannot retrieve the app setting for max discount %-off
    private const int _MAX_AWARD_VALUE_PCT = 2000;

    internal Dictionary<string, Dictionary<string, string>> AvailableCurrencies = new Dictionary<string, Dictionary<string, string>>();
  
    public PrivateLabelPromoCurrency() { }

    public PrivateLabelPromoCurrency(int awardValue, AwardType awardType, string currency, int minSubtotal)
    {
      this.AwardValue = awardValue;
      this.TypeOfAward = awardType;
      this.CurrencyType = currency;
      this.MinSubtotal = minSubtotal;

      AvailableCurrencies = DataCache.DataCache.GetCurrencyDataAll();
    }

    /// <summary>
    /// Validates that a PrivateLabelPromoCurrency object is valid by checking: (1) for promos that are greater than
    /// 100% off the toal value, (2) for promos where "DollarOff" is the AwardType and the AwardValue is greater than the
    /// MinSubtotal, and (3) for promos where the CurrencyType is not available in the list of transactional currencies
    /// available from DataCache. NOTE: As of 7/16/2012 there is no mechanism to check if a CurrencyType is turned "on" 
    /// for PL-awareness.
    /// </summary>
    /// <param name="promoCurrency"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public static bool IsPromoCurrencyValid(PrivateLabelPromoCurrency promoCurrency, ref OrderLevelPromoException exception)
    {
      bool result = true;
      exception = null;

      //check for nulls
      if (promoCurrency == null)
      {
        result = false;
        exception = new OrderLevelPromoException("The promoCurrency supplied to this method was null", OrderLevelPromoExceptionReason.Other);
      }

      //check for unspecified values
      else if (promoCurrency.TypeOfAward == AwardType.Unspecified)
      {
        result = false;
        exception = new OrderLevelPromoException("You did not specifiy a type of award. Available options are \"DollarOff\" and \"PercentOff\"", OrderLevelPromoExceptionReason.InvalidOrUnspecifiedAward);
      }

      //check for zero
      else if (promoCurrency.AwardValue == 0 || promoCurrency.MinSubtotal == 0)
      {
        result = false;
        exception = new OrderLevelPromoException("You cannot set an \"AwardValue\" and/or a \"MinSubtotal\" to zero.", OrderLevelPromoExceptionReason.InvalidOrUnspecifiedAward);
      }

      // Check if the value of the promo > 100% (for %-off promos)
      else if (promoCurrency.TypeOfAward == AwardType.PercentOff && promoCurrency.AwardValue > 10000)
      {
        result = false;
        exception = new OrderLevelPromoException("You cannot allocate more than 100% off of an order. Please check the 'AwardValue' field.", OrderLevelPromoExceptionReason.AwardValueGreaterThanAllowed);
      }

      // Check if the value of the promo is greater than the min required amount (for $$-off promos)
      else if (promoCurrency.TypeOfAward == AwardType.DollarOff && (promoCurrency.AwardValue > promoCurrency.MinSubtotal))
      {
        result = false;
        exception = new OrderLevelPromoException("You cannot allocate a greater monetary discount than the minimum order subtotal. Please check the 'AwardValue' and 'MinSubtotal' fields.", OrderLevelPromoExceptionReason.AwardValueGreaterThanAllowed);
      }

      // Check if currency exists
      else if (promoCurrency.CurrencyType.ToUpper() != "USD" && !promoCurrency.AvailableCurrencies.ContainsKey(promoCurrency.CurrencyType.ToUpper()))
      {
        result = false;
        exception = new OrderLevelPromoException("You cannot specify an invalid currency type. Please check the 'CurrencyType' field.", OrderLevelPromoExceptionReason.InvalidCurrencySpecification);
      }

      //check if award amount is > than max allowed
      else if (promoCurrency.TypeOfAward == AwardType.PercentOff & (promoCurrency.AwardValue > promoCurrency.MaxAwardValuePercentOff))
      {
        result = false;
        exception = new OrderLevelPromoException("You cannot specify a discount amount greater than the system will allow. Specified=[" + promoCurrency.AwardValue + "], MaxAllowable=[" + promoCurrency.MaxAwardValuePercentOff + "]", OrderLevelPromoExceptionReason.InvalidOrUnspecifiedAward);
      }

      else if (promoCurrency.TypeOfAward == AwardType.DollarOff)
      {

        int actualPrice = (promoCurrency.MinSubtotal - promoCurrency.AwardValue);
        double actualPctOff = (actualPrice / promoCurrency.MinSubtotal) * 10000;

        if (actualPctOff > promoCurrency.MaxAwardValuePercentOff)
        {
          result = false;
          exception = new OrderLevelPromoException("You cannot specify a discount amount greater than the system will allow. Specified (as a %)=[" + actualPctOff + "], MaxAllowable=[" + promoCurrency.MaxAwardValuePercentOff + "]", OrderLevelPromoExceptionReason.InvalidOrUnspecifiedAward);
        }
      }

      return result;
    }

    /// <summary>
    /// AwardValue corresponds to the dollar or percentage amount that will be deducted from the order when the promo is
    /// utilized. The numeric format is integer and corresponds to two precision points even though no decimal place is used
    /// in the field. For example, "5000" is equivalent to $50.00 when the AwardType is "DollarOff" and the CurrencyType is USD,
    /// and "5000" is equivalent to 50.00% when the AwardType is "PercentOff", irrespective of the CurrencyType. 
    /// </summary>
    public int AwardValue { get; set; }

    /// <summary>
    /// MinSubtotal corresponds to the minimum balance that is required to be in the cart in order for the promo to take effect.
    /// </summary>
    public int MinSubtotal { get; set; }

    /// <summary>
    /// Gets the system default maximum % off of any order level promo (irrespective of whether % off or % off from app setting)
    /// </summary>
    private int _maxAwardValuePercentOff = _MAX_AWARD_VALUE_PCT;
    public int MaxAwardValuePercentOff
    {
      get
      {
        try
        {
          int result = 0;
          string cacheValue = DataCache.DataCache.GetAppSetting("ORDLEVEL_PROMO_MAX_DISCOUNT");

          if (!string.IsNullOrEmpty(cacheValue))
          {
            if (int.TryParse(cacheValue, out result))
            {
              this._maxAwardValuePercentOff = result;
            }
          }
        }
        catch
        {
        }

        return this._maxAwardValuePercentOff;
      }
    }

    private string _currencyType = "USD";
    /// <summary>
    /// Corresponds to the 3-letter string identifier of the currency being placed against the promotion. The default is "USD".
    /// </summary>
    public string CurrencyType 
    {
      get { return this._currencyType; }
      set { this._currencyType = value; }
    }

    private AwardType _awardType = AwardType.Unspecified;
    /// <summary>
    /// The type of award being placed on the order-level promo. There are currently only 2 available values:
    /// "DollarOff" and "PercentOff". The default value is "Unspecified"
    /// </summary>
    public AwardType TypeOfAward
    { get { return this._awardType; } set { this._awardType = value; } }
    }
}
