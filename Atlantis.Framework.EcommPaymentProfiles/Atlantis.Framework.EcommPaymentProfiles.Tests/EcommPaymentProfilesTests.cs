using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.EcommPaymentProfiles.Interface;

namespace Atlantis.Framework.EcommPaymentProfiles.Tests
{
  [TestClass]
  public class GetEcommPaymentProfilesTests
  {

    private const string _shopperId = "856907";
    private const int _requestType = 384;


    public GetEcommPaymentProfilesTests()
    { }

    public TestContext TestContext { get; set; }

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
    public void EcommPaymentProfilesTest()
    {
      EcommPaymentProfilesRequestData request = new EcommPaymentProfilesRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0);

      EcommPaymentProfilesResponseData response = (EcommPaymentProfilesResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(string.Format("Profile Count: {0}", response.PaymentProfileCount));

      for (int i=0; i<response.PaymentProfileCount; i++)
      {
        PaymentProfile pp;
        if (i % 2 == 0)
        {
          pp = response.AccessProfile(i, _shopperId, null, null, "EcommPaymentProfilesTest");
        }
        else
        {
          pp = response.AccessProfile(i, _shopperId, "1342", "Turd Ferguson", "EcommPaymentProfilesTest");
        }
        Debug.WriteLine(string.Format("ProfileID: {0} | ProfileType: {1} | CreditCardType: {2} | CreditCardNumber: {3}"
          , pp.ProfileID, pp.ProfileType, pp.CreditCardType, pp.CreditCardNumber));
      }

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
