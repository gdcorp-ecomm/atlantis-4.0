using System;
using System.Collections.Generic;
using Atlantis.Framework.FaxEmailAddonPacks.Interface.Types;
using Atlantis.Framework.FaxEmailApplyAddonPack.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.FaxEmailApplyAddonPack.Tests
{
  [TestClass]
  public class FaxEmailApplyAddonPackTests
  {
    public FaxEmailApplyAddonPackTests()
    {
    }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestApplyAddonPack()
    {
      const string fteAccountUid = "411ceae2-193d-11e1-a564-0050569575d8";
      const int fteResourceId = 431774; //must be the resourceid for the fteAccountUid acct
      const int addonPackResourceId = 999999; // must be for the fte type (standard or tollfree) of the fte acct
      DateTime addonPackExpireDate = DateTime.Today;
      var packDetails = new Dictionary<string, string> {{FaxEmailAddonPack.Minutes, "100"}};

      var request = new FaxEmailApplyAddonPackRequestData("857527",
                                                          "sourceUrl",
                                                          "orderId",
                                                          "pathway",
                                                          1,
                                                          1,
                                                          "FaxEmailApplyAddonPackTests",
                                                          "MYA",
                                                          fteAccountUid,
                                                          fteResourceId,
                                                          new FaxEmailAddonPack(addonPackResourceId, addonPackExpireDate, packDetails));
      var response = Engine.Engine.ProcessRequest(request, 460) as FaxEmailApplyAddonPackResponseData;

      Assert.IsNotNull(response);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
