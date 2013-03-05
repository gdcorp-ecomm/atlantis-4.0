using Atlantis.Framework.PrivateLabel.Interface.Base;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class PrivateLabelTypeRequestData : RequestDataUsingPrivateLabelId
  {
    public PrivateLabelTypeRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount, privateLabelId)
    {

    }
  }
}
