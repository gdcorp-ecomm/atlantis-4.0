using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoOrderLevelCreate.Interface
{
  /// <summary>
  /// Defines an instance of an Order-Level promotion that will be passed to the Promo API for creation.
  /// </summary>
  public class OrderLevelPromo
  {
    // per fastball dev team, the "serverGroupId" field is completely ignored in the current
    // implementation of the promo api - however, setting the field to the default value of '0'.
    private const int _PROMO_SERVER_GROUP_ID = 0;

    // per fastball dev team, there are currently only two valid values of "columnType" field:
    // 1=ISC (normal order promo) and 2=FASTBALL. Since this trriplet is about order level promos
    // only we are using a constant for 'ISC'.
    private const int _PROMO_COLUMN_TYPE = 1;

    public OrderLevelPromo() 
    {
      this._resellerTypeList.Add(ResellerType.BasicReseller, true);
      this._resellerTypeList.Add(ResellerType.ProReseller, true);
      this._resellerTypeList.Add(ResellerType.SuperReseller, true);
    }

    public OrderLevelPromo(string promoId, string startDate, string endDate, bool isActive, string iscCode, string iscDescription)
    {
      this._resellerTypeList.Add(ResellerType.BasicReseller, true);
      this._resellerTypeList.Add(ResellerType.ProReseller, true);
      this._resellerTypeList.Add(ResellerType.SuperReseller, true);

      this._promoId = promoId;
      this._startDate = startDate;
      this._endDate = endDate;
      this._iscCode = iscCode;
      this._isActive = isActive;
      this._iscDescription = iscDescription;
    }

    /// <summary>
    /// List of available reseller types that an order-level promo can be applied to.
    /// </summary>
    public enum ResellerType
    {
      GoDaddy = 1,
      ProReseller = 2,
      APIReseller = 3,
      JetDomains = 4,
      SuperReseller = 5,
      BlueRazor = 6,
      WildWestDomains = 7,
      DomainsbyProxy = 8,
      DomainsOnlyReseller = 9,
      SSLOnlyReseller = 10,
      BasicReseller = 11,
      GoDaddyRegistryPortal = 12
    }

    private string _promoId = string.Empty;
    /// <summary>
    /// Gets or sets the ID of the promotion that will physically be used during the purchase process by the consumer
    /// of the promotion.
    /// </summary>
    public string PromoId
    {
      get { return this._promoId; }
      set { this._promoId = value; }
    }

    private string _startDate = string.Empty;
    /// <summary>
    /// Gets or sets the start date of the promotion
    /// </summary>
    public string StartDate
    {
      get { return this._startDate; }
      set { this._startDate = value; }
    }

    private string _endDate = string.Empty;
    /// <summary>
    /// Gets or sets the end date of the promotion
    /// </summary>
    public string EndDate
    {
      get { return this._endDate; }
      set{ this._endDate = value; }
    }

    protected internal int ServerGroupId
    {
      get { return _PROMO_SERVER_GROUP_ID; }
    }

    protected internal int ColumnType
    {
      get { return _PROMO_COLUMN_TYPE; }
    }

    private List<PrivateLabelPromoCurrency> _currencies = new List<PrivateLabelPromoCurrency>();
    /// <summary>
    /// Gets or sets the list of PrivateLabelPromoCurrency instances that the promotion will be applied to upon creation.
    /// </summary>
    public List<PrivateLabelPromoCurrency> Currencies
    {
      get { return this._currencies; }
      set { this._currencies = value; }
    }

    private bool _isActive = false;

      private bool _skipvalidation = false;
    /// <summary>
    /// Gets or sets whether the promo is to be activated upon creation.
    /// </summary>
    public bool IsActive
    {
      get { return this._isActive; }
      set { this._isActive = value; }
    }

    private string _iscCode = string.Empty;
    /// <summary>
    /// Gets or sets the tracking code that will be attached to the order-level promo.
    /// </summary>
    public string ISCCode
    {
      get { return this._iscCode; }
      set { this._iscCode = value; }
    }

    private string _iscDescription = string.Empty;
    /// <summary>
    /// Gets or sets the corresponding description for the tracking code that will be attached to the order-level promo.
    /// </summary>
    public string ISCDescription
    {
      get { return this._iscDescription; }
      set { this._iscDescription = value; }
    }

    public bool SkipValidation
    {
        get { return this._skipvalidation; }
        set { this._skipvalidation = value; }
    }
    private Dictionary<ResellerType, bool> _resellerTypeList = new Dictionary<ResellerType, bool>();

    /// <summary>
    /// Gets or sets the list of ResellerTypes that this promo will be applied to and identifies whether the 
    /// specified reseller type will be active or not upon creation.
    /// </summary>
    public Dictionary<ResellerType, bool> ResellerTypeList
    {
      get
      {
        return this._resellerTypeList;
      }
      set
      {
        this._resellerTypeList = value;
      }
    }

    /// <summary>
    /// Checks whether the specified string can successfully be parsed into a valid DateTime format.
    /// </summary>
    /// <param name="dateToCheck">The string to attempt to parse out into a date.</param>
    /// <returns>Boolean value indicating whether the string can be successfully parsed into a date.</returns>
    public static bool IsValidDate(string dateToCheck)
    {
      DateTime parsedOut;
      return DateTime.TryParse(dateToCheck, out parsedOut);
    }

    /// <summary>
    /// Checks whether the string can successfully be parsed into a valid DateTime format and, if so, whether
    /// the date is in the future, relative to current date.
    /// </summary>
    /// <param name="dateToCheck">The string representation to try and parse to a date</param>
    /// <returns></returns>
    public static bool IsDateInFuture(string dateToCheck)
    {
      DateTime parseDate;
      bool result = false;

      if (DateTime.TryParse(dateToCheck, out parseDate))
      {
        if (DateTime.Now.Date <= parseDate.Date)
        {
          result = true;
        }
      }

      return result;
    }

  }
}
