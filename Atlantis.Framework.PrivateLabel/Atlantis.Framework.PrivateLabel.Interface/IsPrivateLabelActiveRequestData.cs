using Atlantis.Framework.PrivateLabel.Interface.Base;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class IsPrivateLabelActiveRequestData : RequestDataUsingPrivateLabelId
  {
    public IsPrivateLabelActiveRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount, privateLabelId)
    {

    }
  }
}
