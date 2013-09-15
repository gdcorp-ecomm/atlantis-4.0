using Atlantis.Framework.PrivateLabel.Interface.Base;
using System;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class IsPrivateLabelActiveRequestData : RequestDataUsingPrivateLabelId
  {
    [Obsolete("Please use the simpler constructor")]
    public IsPrivateLabelActiveRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId)
      : this(privateLabelId)
    {
    }

    public IsPrivateLabelActiveRequestData(int privateLabelId)
      : base(privateLabelId)
    {
    }
  }
}
