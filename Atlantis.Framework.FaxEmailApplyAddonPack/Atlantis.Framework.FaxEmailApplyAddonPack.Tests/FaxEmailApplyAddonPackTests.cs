using System;
using System.Collections.Generic;
using Atlantis.Framework.FaxEmailAddonPacks.Interface.Types;
using Atlantis.Framework.FaxEmailApplyAddonPack.Impl;
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
    public void TestGetActionXml()
    {
      const int resourceId = 1;
      const string shopperId = "2";
      const int privateLabelId = 3;
      const string externalResourceId = "4";
      const int addonResourceid = 5;
      const string attributeUid = "6";
      const string enteredby = "7";

      string xml = FaxEmailApplyAddonPackRequest.GetActionXml(resourceId, shopperId, privateLabelId, externalResourceId, addonResourceid, attributeUid, enteredby);
      Assert.Equals(xml, "<ACTIONROOT><ACTION id=\"1\" privatelabelid=\"3\" shopper_id=\"2\" name=\"FaxEmailAddMinutes\" /><FAXEMAIL child_resource_id=\"5\" external_resource_id=\"4\" child_external_resource_id=\"6\" /><NOTES><SHOPPERNOTE note=\"FaxThruEmail Minute Pack consumed\" enteredby=\"7\" /><ACTIONNOTE note=\"REQUESTEDBY: 7\nFaxThruEmail Minute Pack consumed\" modifiedby=\"14\" /></NOTES></ACTIONROOT>");
    }

    [TestMethod]
    public void TestGetErrorMessage()
    {
      const string expectedMessage = "Error message";
      string input = String.Format("<Status><Error>-1</Error><Description>{0}</Description></Status>", expectedMessage);
      string actualMessage = FaxEmailApplyAddonPackRequest.GetErrorMessage(input);
      Assert.Equals(actualMessage, expectedMessage);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestApplyAddonPack()
    {
      const string fteAccountUid = "411ceae2-193d-11e1-a564-0050569575d8";
      const int fteResourceId = 431774; //must be the resourceid for the fteAccountUid acct
      const int addonPackResourceId = 431771; // must be for the fte type (standard or tollfree) of the fte acct
      DateTime addonPackExpireDate = DateTime.Parse("2012-11-27 00:00:00.000");
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
