using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.BuyerProfileAddUpdate.Interface;
using Atlantis.Framework.BuyerProfileDetails.Interface;

namespace Atlantis.Framework.BuyerProfileAddUpdate.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class ProfileSave
  {
    public ProfileSave()
    {
      //
      // TODO: Add constructor logic here
      //
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
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]    
    public void TestMethod1()
    {
      ProfileAdd();
      //ProfileUpdate();

    }

    private void ProfileAdd()
    {
      AddressList addlist1 = new AddressList(0, string.Empty, string.Empty, "sridhar", string.Empty, "tho", string.Empty, "add11", "add2", "scott", "arizona", "85260", "United States", "9897898765", "9878978789", string.Empty, "sthota@godaddy.com");
      AddressList addlist2 = new AddressList(1, string.Empty, string.Empty, "sridhar", string.Empty, "tho", string.Empty, "add12", "add2", "scott", "arizona", "85260", "United States", "9897898765", "9878978789", string.Empty, "sthota@godaddy.com");
      AddressList addlist3 = new AddressList(2, string.Empty, string.Empty, "sridhar", string.Empty, "tho", string.Empty, "add13", "add2", "scott", "arizona", "85260", "United States", "9897898765", "9878978789", string.Empty, "sthota@godaddy.com");
      AddressList addlist4 = new AddressList(3, string.Empty, string.Empty, "sridhar", string.Empty, "tho", string.Empty, "add14", "add2", "scott", "arizona", "85260", "United States", "9897898765", "9878978789", string.Empty, "sthota@godaddy.com");

      List<AddressList> addlist = new List<AddressList>(4);
      addlist.Add(addlist1);
      addlist.Add(addlist2);
      addlist.Add(addlist3);
      addlist.Add(addlist4);

      List<string> nameservers = new List<string>();
      ProfileDetail profile = new ProfileDetail("profiletest-new2", 2, false, true, false, false, addlist, nameservers);
      BuyerProfileAddUpdateRequestData request = new BuyerProfileAddUpdateRequestData("850774", string.Empty, string.Empty, string.Empty, 0);
      request.IsNewBuyerProfile = true;
      request.BuyerProfile = profile;

      BuyerProfileAddUpdateResponseData response = (BuyerProfileAddUpdateResponseData)Engine.Engine.ProcessRequest(request, 412);

      Assert.IsTrue(response.IsSuccess); 
    }

    private void ProfileUpdate()
    {
      AddressList addlist1 = new AddressList(0, string.Empty, string.Empty, "sridhar", string.Empty, "tho", string.Empty, "add11", "add2", "scott", "arizona", "85260", "United States", "9897898765", "9878978789", string.Empty, "sthota@godaddy.com");
      AddressList addlist2 = new AddressList(1, string.Empty, string.Empty, "sridhar", string.Empty, "tho", string.Empty, "add12", "add2", "scott", "arizona", "85260", "United States", "9897898765", "9878978789", string.Empty, "sthota@godaddy.com");
      AddressList addlist3 = new AddressList(2, string.Empty, string.Empty, "sridhar", string.Empty, "tho", string.Empty, "add13", "add2", "scott", "arizona", "85260", "United States", "9897898765", "9878978789", string.Empty, "sthota@godaddy.com");
      AddressList addlist4 = new AddressList(3, string.Empty, string.Empty, "sridhar", string.Empty, "tho", string.Empty, "add14", "add2", "scott", "arizona", "85260", "United States", "9897898765", "9878978789", string.Empty, "sthota@godaddy.com");

      List<AddressList> addlist = new List<AddressList>(4);
      addlist.Add(addlist1);
      addlist.Add(addlist2);
      addlist.Add(addlist3);
      addlist.Add(addlist4);

      List<string> nameservers = new List<string>();
      ProfileDetail profile = new ProfileDetail("us1", 2, false, true, false, false, addlist, nameservers);
      BuyerProfileAddUpdateRequestData request = new BuyerProfileAddUpdateRequestData("850774", string.Empty, string.Empty, string.Empty, 0);
      request.IsNewBuyerProfile = false;
      request.ProfileID = "150304";
      request.BuyerProfile = profile;

      BuyerProfileAddUpdateResponseData response = (BuyerProfileAddUpdateResponseData)Engine.Engine.ProcessRequest(request, 412);

      Assert.IsTrue(response.IsSuccess); 
    }
  }
}
