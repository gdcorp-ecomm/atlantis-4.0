using Atlantis.Framework.PrivateLabel.Interface.Base;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class PLSignupInfoRequestData : RequestDataUsingPrivateLabelId
  {
    public PLSignupInfoRequestData(int privateLabelId) : base(privateLabelId)
    {
    }
  }
}
