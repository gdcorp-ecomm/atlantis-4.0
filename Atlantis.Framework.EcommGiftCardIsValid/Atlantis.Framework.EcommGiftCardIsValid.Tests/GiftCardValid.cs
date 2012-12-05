using Atlantis.Framework.EcommGiftCardIsValid.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommGiftCardIsValid.Tests
{
  [TestClass]
  public class GiftCardValid
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.EcommGiftCardIsValid.Impl.dll")]
    public void GiftCardIsValid()
    {
      string cardNumber = "785965352214";
      EcommGiftCardIsValidRequestData requestData = new EcommGiftCardIsValidRequestData("850774", string.Empty, string.Empty, string.Empty, 0, cardNumber);
      EcommGiftCardIsValidResponseData responseData = (EcommGiftCardIsValidResponseData)Engine.Engine.ProcessRequest(requestData, 625);
      bool isValidCard = responseData.IsGiftCardValid;
      Assert.IsTrue(responseData.IsSuccess);
    }
  }
}
