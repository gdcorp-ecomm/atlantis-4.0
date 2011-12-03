using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CostcoSetMemberInfo.Interface;
using System;

namespace Atlantis.Framework.CostcoSetMemberInfo.Tests
{
  /// <summary>
  /// This performs the following tests:
  /// 
  /// 1. Fails for BlueRazor w/valid Shopper ID & Costco member information, and correct zipcode.
  /// 2. Fails for WildWest w/valid Shopper ID & Costco member information, and correct zipcode.
  /// 3. Fails for a Private Label w/valid Shopper ID & Costco member information, and correct zipcode.
  /// 4. Succeeds for GoDaddy w/valid Shopper ID & Costco member information, and correct zipcode.
  /// 5. Fails for GoDaddy w/invalid Shopper ID & valid Costco member information, and correct zipcode.
  /// 6. Fails for GoDaddy w/valid Shopper ID & invalid Costco member information, and correct zipcode.
  /// 7. Fails for GoDaddy w/valid Shopper ID & valid Costco member information, but wrong zipcode.
  /// 8. Succeeds for GoDaddy w/valid BlueRazor shopperId & Costco member information, and correct zipcode.
  /// 9. Fails for GoDaddy w/valid Shopper ID & valid but expired Costco member information, and correct zipcode.
  /// 
  /// </summary>
  [TestClass]
  public class CostcoSetMemberInfoTests
  {
    public CostcoSetMemberInfoTests()
    {
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes

    private int _engineRequestId = 367;

    private string validCostcoMembershipId        = "111757199605";
    private string validExpiredCostcoMembershipId = "111757210145";
    private string invalidCostcoMembershipId      = "999999999999";

    private string validZipcodeForValidCostcoMembershipId   = "98027";
    private string invalidZipcodeForValidCostcoMembershipId = "85000";

    private string validGoDaddyShopperId      = "863301";
    private string validGoDaddyDDCShopperId   = "853516";
    private string validBlueRazorShopperId    = "855341";
    private string validWWDShopperId          = "858791";
    private string validPrivateLabelShopperId = "860303";

    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestBlueRazor()
    {
      int resellerId = 2; // BlueRazor
      var request = new CostcoSetMemberInfoRequestData(validBlueRazorShopperId, string.Empty, string.Empty, string.Empty, 0,
                                             validCostcoMembershipId,
                                             validZipcodeForValidCostcoMembershipId,
                                             resellerId);
      try
      {
        var response = (CostcoSetMemberInfoResponseData)Engine.Engine.ProcessRequest(request, _engineRequestId);
        Assert.IsTrue(false); // should never hit this line
      }
      catch (Exception e)
      {
        Assert.IsTrue(e.Message.Contains("Invalid Private Label"));
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestBlueRazorShopperWithGoDaddyReseller()
    {
      int resellerId = 1; // GoDaddy
      var request = new CostcoSetMemberInfoRequestData(validBlueRazorShopperId, string.Empty, string.Empty, string.Empty, 0,
                                             validCostcoMembershipId,
                                             validZipcodeForValidCostcoMembershipId,
                                             resellerId);
      var response = (CostcoSetMemberInfoResponseData)Engine.Engine.ProcessRequest(request, _engineRequestId);
      Assert.IsFalse(response.IsSuccess); // BlueRazor Shoppers are represented as DDC apparently in the database.
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestWildWest()
    {
      int resellerId = 1387; // Wild West Domains
      var request = new CostcoSetMemberInfoRequestData(validWWDShopperId, string.Empty, string.Empty, string.Empty, 0,
                                             validCostcoMembershipId,
                                             validZipcodeForValidCostcoMembershipId,
                                             resellerId);
      try
      {
        var response = (CostcoSetMemberInfoResponseData)Engine.Engine.ProcessRequest(request, _engineRequestId);
        Assert.IsTrue(false); // should never hit this line
      }
      catch (Exception e)
      {
        Assert.IsTrue(e.Message.Contains("Invalid Private Label"));
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestPrivateLabel()
    {
      int resellerId = 440253; // progid=bluefin
      var request = new CostcoSetMemberInfoRequestData(validPrivateLabelShopperId, string.Empty, string.Empty, string.Empty, 0,
                                             validCostcoMembershipId,
                                             validZipcodeForValidCostcoMembershipId,
                                             resellerId);
      try
      {
        var response = (CostcoSetMemberInfoResponseData)Engine.Engine.ProcessRequest(request, _engineRequestId);
        Assert.IsTrue(false); // should never hit this line
      }
      catch (Exception e)
      {
        Assert.IsTrue(e.Message.Contains("Invalid Private Label"));
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestGoDaddySuccess()
    {
      int resellerId = 1; // GoDaddy
      var request = new CostcoSetMemberInfoRequestData(validGoDaddyShopperId, string.Empty, string.Empty, string.Empty, 0,
                                             validCostcoMembershipId,
                                             validZipcodeForValidCostcoMembershipId,
                                             resellerId);
      var response = (CostcoSetMemberInfoResponseData)Engine.Engine.ProcessRequest(request, _engineRequestId);

      if ( response.IsSuccess )
      {
        // more likely branch
        Assert.IsTrue(true);
      }
      else
      {
        Assert.IsTrue(response.ExistingMember.HasValue && response.ExistingMember.Value);
      }

      Assert.IsTrue(response.MemberLevel.HasValue && response.MemberLevel.Value > 0);
      Assert.IsTrue(response.DiscountDomainClub.HasValue && !response.DiscountDomainClub.Value);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestGoDaddyRandomShopperId()
    {
      int resellerId = 1; // GoDaddy
      string invalidShopperId = String.Concat("junk", new Random(Convert.ToInt32(DateTime.Now.Ticks % int.MaxValue)).Next(99999));
      var request = new CostcoSetMemberInfoRequestData(invalidShopperId, string.Empty, string.Empty, string.Empty, 0,
                                             validCostcoMembershipId,
                                             validZipcodeForValidCostcoMembershipId,
                                             resellerId);
      var response = (CostcoSetMemberInfoResponseData)Engine.Engine.ProcessRequest(request, _engineRequestId);
      if (response.IsSuccess)
      {
        Assert.IsTrue(true);
      }
      else
      {
        // more likely branch
        Assert.IsTrue(response.ExistingMember.HasValue && response.ExistingMember.Value);
      }

      Assert.IsTrue(response.MemberLevel.HasValue && response.MemberLevel.Value > 0);
      Assert.IsTrue(response.DiscountDomainClub.HasValue && !response.DiscountDomainClub.Value);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestGoDaddyBadCostcoId()
    {
      int resellerId = 1; // GoDaddy
      var request = new CostcoSetMemberInfoRequestData(validGoDaddyShopperId, string.Empty, string.Empty, string.Empty, 0,
                                             invalidCostcoMembershipId,
                                             validZipcodeForValidCostcoMembershipId,
                                             resellerId);
      var response = (CostcoSetMemberInfoResponseData)Engine.Engine.ProcessRequest(request, _engineRequestId);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.Response.Contains("Invalid Membership Number"));//Invalid Membership Number.
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestGoDaddyExpiredCostcoId()
    {
      int resellerId = 1; // GoDaddy
      var request = new CostcoSetMemberInfoRequestData(validGoDaddyShopperId, string.Empty, string.Empty, string.Empty, 0,
                                             validExpiredCostcoMembershipId,
                                             validZipcodeForValidCostcoMembershipId,
                                             resellerId);
      var response = (CostcoSetMemberInfoResponseData)Engine.Engine.ProcessRequest(request, _engineRequestId);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.MessageForUser.Contains("Account Expired")); //Account Expired. Last Renewal = 2005-01-31.
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestGoDaddyBadZipcode()
    {
      int resellerId = 1; // GoDaddy
      var request = new CostcoSetMemberInfoRequestData(validGoDaddyShopperId, string.Empty, string.Empty, string.Empty, 0,
                                             validCostcoMembershipId,
                                             invalidZipcodeForValidCostcoMembershipId,
                                             resellerId);
      var response = (CostcoSetMemberInfoResponseData)Engine.Engine.ProcessRequest(request, _engineRequestId);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.MessageForUser.Contains("Validation zip code")); //>Validation zip code doesn't match Costco's zip code.
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestGoDaddyDDCShopper()
    {
      int resellerId = 1; // GoDaddy
      var request = new CostcoSetMemberInfoRequestData(validGoDaddyDDCShopperId, string.Empty, string.Empty, string.Empty, 0,
                                             validCostcoMembershipId,
                                             validZipcodeForValidCostcoMembershipId,
                                             resellerId);
      var response = (CostcoSetMemberInfoResponseData)Engine.Engine.ProcessRequest(request, _engineRequestId);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.DiscountDomainClub.HasValue && response.DiscountDomainClub.Value);
    }

  }
}
