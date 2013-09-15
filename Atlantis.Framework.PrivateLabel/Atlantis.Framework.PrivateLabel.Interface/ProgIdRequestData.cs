using Atlantis.Framework.PrivateLabel.Interface.Base;
using System;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class ProgIdRequestData : RequestDataUsingPrivateLabelId
  {
    [Obsolete("Please use the simpler constructor.")]
    public ProgIdRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId)
      : this(privateLabelId)
    {
    }

    public ProgIdRequestData(int privateLabelId)
      : base(privateLabelId)
    {
    }
  }
}
