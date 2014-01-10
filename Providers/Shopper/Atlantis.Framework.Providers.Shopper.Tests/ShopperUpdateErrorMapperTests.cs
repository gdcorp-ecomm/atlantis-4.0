using Atlantis.Framework.Providers.Shopper.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Shopper.Tests
{
  [TestClass]
  public class ShopperUpdateErrorMapperTests
  {
    [TestMethod]
    public void ShopperUpdatErrorMapperMatch()
    {
      var result = ShopperUpdateErrorMapper.GetUpdateResultType("0xC0044A20");
      Assert.AreEqual(ShopperUpdateResultType.PasswordUnacceptable, result);
    }

    [TestMethod]
    public void ShopperUpdatErrorMapperNoMatch()
    {
      var result = ShopperUpdateErrorMapper.GetUpdateResultType("thisisnotamatch");
      Assert.AreEqual(ShopperUpdateResultType.UnknownError, result);
    }
  }
}
