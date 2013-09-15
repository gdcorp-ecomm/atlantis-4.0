using Atlantis.Framework.PrivateLabel.Interface.Base;
using System;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class PrivateLabelTypeRequestData : RequestDataUsingPrivateLabelId
  {
    [Obsolete("Please use the simpler constructor.")]
    public PrivateLabelTypeRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId)
      : this(privateLabelId)
    {
    }

    public PrivateLabelTypeRequestData(int privateLabelId)
      : base(privateLabelId)
    {
    }
  }
}
