using Atlantis.Framework.DCCGetDomainByShopper.Interface;
using Atlantis.Framework.DCCGetDomainByShopper.Interface.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DCCGetDomainByShopper.Tests
{

  [TestClass()]
  public class DCCGetDomainByShopperRequestDataTest
  {

    [TestMethod()]
    public void GetCacheMD5Test()
    {

      IDomainPaging paging = new MinimalSummaryOnlyPaging();
      var request = new DCCGetDomainByShopperRequestData("859148", string.Empty, string.Empty, string.Empty, 0, paging, "MOBILE_CSA_DCC");
      string md5_1 = request.GetCacheMD5();
      string md5_1a = request.GetCacheMD5();
      Assert.AreEqual(md5_1, md5_1a, "Repeated calls to GetCachedMD5 do not generate the same response.");

      var request2 = new DCCGetDomainByShopperRequestData("859148", string.Empty, string.Empty, string.Empty, 0, paging, "MOBILE_CSA_DCC");
      request2.DbpFilter = DCCGetDomainByShopperRequestData.DomainByProxyFilter.DbpOnly;
      string md5_2 = request2.GetCacheMD5();
      Assert.AreNotEqual(md5_1, md5_2, "Changes to DbpFilter (DbpOnly) do not generate different md5s.");

      var request3 = new DCCGetDomainByShopperRequestData("859148", string.Empty, string.Empty, string.Empty, 0, paging, "MOBILE_CSA_DCC");
      request3.DbpFilter = DCCGetDomainByShopperRequestData.DomainByProxyFilter.NoDbpOnly;
      string md5_3 = request2.GetCacheMD5();
      Assert.AreNotEqual(md5_1, md5_3, "Changes to DbpFilter (NoDbpOnly) do not generate different md5s.");
    }
  }
}
