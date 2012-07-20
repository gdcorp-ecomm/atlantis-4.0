using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.AddOrderLevelPromoPL.Interface
{
  public enum PLOrderLevelPromoExceptionReason
  {
    Unknown = 0,
    PromoAlreadyExists = 1,
    ImproperRequestFormat = 2,
    Other = 3,
    InvalidDateFormat = 4,
    InvalidDateRange = 5,
    AwardValueGreaterThanAllowed = 6,
    InvalidOrUnspecifiedAward = 7,
    InvalidCurrencySpecification = 8
  }

  [Serializable]
  public class PLOrderLevelPromoException : Exception
  {
    private PLOrderLevelPromoExceptionReason _reason = PLOrderLevelPromoExceptionReason.Unknown;

    public PLOrderLevelPromoException(string message)
      : base(message)
    {
    }

    public PLOrderLevelPromoException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public PLOrderLevelPromoException(string message, PLOrderLevelPromoExceptionReason reason)
      : base(message)
    {
      this._reason = reason;
    }

    public PLOrderLevelPromoException(string message, Exception innerException, PLOrderLevelPromoExceptionReason reason)
      : base(message, innerException)
    {
      this._reason = reason;
    }

    protected PLOrderLevelPromoException(SerializationInfo info, StreamingContext context)
    : base(info, context)
    {
      if (info != null)
      {
        this._reason = (PLOrderLevelPromoExceptionReason) Enum.Parse(typeof(PLOrderLevelPromoExceptionReason), info.GetString("_reason"));
      }
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);

      if (info != null)
      {
        info.AddValue("_reason", Enum.GetName(typeof(PLOrderLevelPromoExceptionReason), this._reason));
      }
    }

    public PLOrderLevelPromoExceptionReason ExceptionReason
    {
      get { return this._reason; }
    }
  }
}
