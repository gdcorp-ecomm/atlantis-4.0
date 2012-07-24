using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.PromoOrderLevelCreate.Interface
{
  public enum OrderLevelPromoExceptionReason
  {
    Unknown = 0,
    PromoAlreadyExists = 1,
    ImproperRequestFormat = 2,
    Other = 3,
    InvalidDateFormat = 4,
    InvalidDateRange = 5,
    AwardValueGreaterThanAllowed = 6,
    InvalidOrUnspecifiedAward = 7,
    InvalidCurrencySpecification = 8,
    InvalidPromoGeneric = 9
  }

  [Serializable]
  public class OrderLevelPromoException : Exception
  {
    private OrderLevelPromoExceptionReason _reason = OrderLevelPromoExceptionReason.Unknown;

    public OrderLevelPromoException(string message)
      : base(message)
    {
    }

    public OrderLevelPromoException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public OrderLevelPromoException(string message, OrderLevelPromoExceptionReason reason)
      : base(message)
    {
      this._reason = reason;
    }

    public OrderLevelPromoException(string message, Exception innerException, OrderLevelPromoExceptionReason reason)
      : base(message, innerException)
    {
      this._reason = reason;
    }

    protected OrderLevelPromoException(SerializationInfo info, StreamingContext context)
    : base(info, context)
    {
      if (info != null)
      {
        this._reason = (OrderLevelPromoExceptionReason) Enum.Parse(typeof(OrderLevelPromoExceptionReason), info.GetString("_reason"));
      }
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);

      if (info != null)
      {
        info.AddValue("_reason", Enum.GetName(typeof(OrderLevelPromoExceptionReason), this._reason));
      }
    }

    public OrderLevelPromoExceptionReason ExceptionReason
    {
      get { return this._reason; }
    }
  }
}
