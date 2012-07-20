using System;

namespace Atlantis.Framework.CreatePLOrderLevelPromo.Interface
{
  public class PrivateLabelPromo
  {
    public PrivateLabelPromo(int privateLabelId)
    {
      this._privateLabelId = privateLabelId;
      this._startDate = string.Empty;
      this._endDate = string.Empty;
      this._isActive = false;
    }

    public PrivateLabelPromo(int privateLabelId, string startDate, string endDate, bool isActive)
    {
      this._privateLabelId = privateLabelId;
      this._isActive = isActive;

      this.SetPLPromoStartAndEndDateAndValidate(startDate, endDate);
    }

    private int _privateLabelId;
    /// <summary>
    /// Gets or sets PLID of the reseller that is being appended to a promo
    /// </summary>
    public int PrivateLabelId
    {
      get { return this._privateLabelId; }
      set { this._privateLabelId = value; }
    }

    private string _startDate;
    /// <summary>
    /// Gets or sets start date of the order-level promo AS APPLIED TO THE SPECIFIC PL
    /// </summary>
    public string StartDate
    {
      get { return this._startDate; }
      set { this._startDate = value; }
    }

    private string _endDate;
    /// <summary>
    /// Gets or sets end date of the order-level promo AS APPLIED TO THE SPECIFIC PL
    /// </summary>
    public string EndDate
    {
      get { return this._endDate; }
      set { this._endDate = value; }
    }

    private bool _isActive = false;
    /// <summary>
    /// Gets or sets whether the promo will be active upon creation AS APPLIED TO THE SPECIFIC PL
    /// </summary>
    public bool isActive
    {
      get { return this._isActive; }
      set { this._isActive = value; }
    }

    /// <summary>
    /// Sets the "StartDate" and "EndDate" parameters after first attempting to parse out the strings to
    /// proper DateTime objects.
    /// </summary>
    /// <param name="startDate">The start date for the order-level promo for this reseller</param>
    /// <param name="endDate">The end date for the order-level promo for this reseller</param>
    public void SetPLPromoStartAndEndDateAndValidate(string startDate, string endDate)
    {
      if (!IsDateValid(startDate) || !IsDateValid(endDate))
      {
        throw new ArgumentException("Either the 'startDate' parameter [" + startDate + "] or the 'endDate' parameter [" + endDate + "] are invalid and cannot be cast to a DateTime object.");
      }

      if (!IsDateInFuture(endDate))
      {
        throw new ArgumentException("The 'endDate' specified for the order-level promo must be in the future.", "endDate");
      }

      this._startDate = startDate;
      this._endDate = endDate;
    }

    /// <summary>
    /// Method to validate a date parameter passed in as a string.
    /// </summary>
    /// <param name="dateToValidate">The string to attempt to parse out into a datetime object.</param>
    /// <returns></returns>
    public static bool IsDateValid(string dateToValidate)
    {
      DateTime parsedDate;
      return DateTime.TryParse(dateToValidate, out parsedDate);
    }

    /// <summary>
    /// Attempts to validate whether the string passed in can first be parsed out as a datetime object and subsequently
    /// whether the date is in the future.
    /// </summary>
    /// <param name="dateToValidate">The string to attempt to parse out and verify</param>
    /// <returns></returns>
    public static bool IsDateInFuture(string dateToValidate)
    {
      bool result = false;
      DateTime parsedDate;

      if (DateTime.TryParse(dateToValidate, out parsedDate))
      {
        if (parsedDate.Date >= DateTime.Now.Date)
        {
          result = true;
        }
      }
      return result;
    }

    /// <summary>
    /// Validates the data within the specified PrivateLabelPromo object and returns an exception (if needed).
    /// </summary>
    /// <param name="promo">The PrivateLabelPromo to examine and verify</param>
    /// <param name="exception">The exception identified (if needed) during verification of the object.</param>
    /// <returns>Boolean value specifying whether the PrivateLabelPromo object passed validation or not.</returns>
    public static bool ValidatePrivateLabelPromo(PrivateLabelPromo promo, ref PrivateLabelPromoException exception)
    {
      bool result = true;
      exception = null;

      if (string.IsNullOrEmpty(promo._startDate))
      {
        result = false;
        exception = new PrivateLabelPromoException("Either a null or empty value was set for the 'StartDate' member.", PrivateLabelPromoExceptionReason.RequiredDataMissing);
      }
      else if (string.IsNullOrEmpty(promo._endDate))
      {
        result = false;
        exception = new PrivateLabelPromoException("Either a null or empty value was set for the 'EndDate' member.", PrivateLabelPromoExceptionReason.RequiredDataMissing);
      }
      else if (!IsDateValid(promo.StartDate))
      {
        result = false;
        exception = new PrivateLabelPromoException("The value specified for the 'StartDate' member is invalid [" + promo.StartDate + "].", PrivateLabelPromoExceptionReason.InvalidDateFormat);
      }
      else if (!IsDateValid(promo.EndDate))
      {
        result = false;
        exception = new PrivateLabelPromoException("The value specified for the 'EndDate' member is invalid [" + promo.EndDate + "].", PrivateLabelPromoExceptionReason.InvalidDateFormat);
      }
      else if (!IsDateInFuture(promo.EndDate))
      {
        result = false;
        exception = new PrivateLabelPromoException("The 'EndDate' member must be a date that is in the future [" + promo.EndDate + "].", PrivateLabelPromoExceptionReason.InvalidDateRange);
      }
      else if (promo.PrivateLabelId == 0)
      {
        result = false;
        exception = new PrivateLabelPromoException("The 'PrivateLabelID' member must be specified.", PrivateLabelPromoExceptionReason.RequiredDataMissing);
      }

      return result;
    }
  }
}
