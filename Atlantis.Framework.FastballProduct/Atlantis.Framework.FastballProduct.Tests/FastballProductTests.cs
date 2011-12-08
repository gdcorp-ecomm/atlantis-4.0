using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.FastballProduct.Interface;

namespace Atlantis.Framework.FastballProduct.Tests
{
  [TestClass]
  public class FastballProductTests
  {
    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void SameMD5Hash()
    {
      FastballProductRequestData request1 = new FastballProductRequestData("shoppera", "urla", "ordera", Guid.NewGuid().ToString(), 1, "SamePlacement", 2);
      FastballProductRequestData request2 = new FastballProductRequestData("shopperb", "urlb", "orderb", Guid.NewGuid().ToString(), 2, "SamePlacement", 2);
      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }
  }
}
