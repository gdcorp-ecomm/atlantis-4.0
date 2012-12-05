using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommGiftCardIsCancelled.Interface;

namespace Atlantis.Framework.EcommGiftCardIsCancelled.Tests
{
  [TestClass]
  public class GiftCardCancelled
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    [DeploymentItem("Atlantis.Framework.EcommGiftCardIsCancelled.Impl.dll")]
    public void IsGiftCardCancelled()
    {
      EcommGiftCardIsCancelledRequestData request = new EcommGiftCardIsCancelledRequestData("850774", string.Empty, string.Empty, string.Empty, 0, 9089999);
      EcommGiftCardIsCancelledResponseData response =
        (EcommGiftCardIsCancelledResponseData) Engine.Engine.ProcessRequest(request, 626);
      if (response.IsSuccess)
      {
        bool isCancelled = response.IsGiftCardCancelled;
      }
    }
  }
}
