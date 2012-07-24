using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.PromoOrderLevelAddPL.Interface
{
  public enum PrivateLabelPromoExceptionReason
  {
    Unknown = 0,
    InvalidDateFormat = 1,
    InvalidDateRange = 2,
    RequiredDataMissing = 3,
    InvalidRequestFormat = 4,
    PromoCodeDoesntExist = 5
  }

  [Serializable]
  public class PrivateLabelPromoException : Exception
  {
    private PrivateLabelPromoExceptionReason _reason = PrivateLabelPromoExceptionReason.Unknown;

    public PrivateLabelPromoException()
    {
    }

    public PrivateLabelPromoException(string message)
      : base(message)
    {
    }

    public PrivateLabelPromoException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public PrivateLabelPromoException(string message, Exception innerException, PrivateLabelPromoExceptionReason reason)
      : base(message, innerException)
    {
      this._reason = reason;
    }

    public PrivateLabelPromoException(string message, PrivateLabelPromoExceptionReason reason)
      : base(message)
    {
      this._reason = reason;
    }

    protected PrivateLabelPromoException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      if (info != null)
      {
        this._reason = (PrivateLabelPromoExceptionReason)Enum.Parse(typeof(PrivateLabelPromoExceptionReason), info.GetString("_reason"));
      }
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);

      if (info != null)
      {
        info.AddValue("_reason", Enum.GetName(typeof(PrivateLabelPromoExceptionReason), _reason));
      }
    }

    public PrivateLabelPromoExceptionReason Reason
    {
      get { return this._reason; }
    }
  }
}
