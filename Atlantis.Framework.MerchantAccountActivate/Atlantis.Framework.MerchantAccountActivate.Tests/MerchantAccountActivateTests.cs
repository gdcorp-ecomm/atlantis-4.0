using System.Diagnostics;
using Atlantis.Framework.MerchantAccountActivate.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.MerchantAccountActivate.Tests
{
	[TestClass]
	public class GetMerchantAccountActivateTests
	{
		private const string _shopperId = "856907";
		private const int _requestType = 473;


		public GetMerchantAccountActivateTests()
		{ }

		private TestContext testContextInstance;

		public TestContext TestContext
		{
			get { return testContextInstance; }
			set { testContextInstance = value; }
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
    [DeploymentItem("atlantis.framework.merchantaccountactivate.impl.dll")]
		public void MerchantAccountActivateTest()
		{
			int merchantAccountId = 682;

			MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

			MerchantAccountActivateRequestData request = new MerchantAccountActivateRequestData(_shopperId
				, string.Empty
				, string.Empty
				, string.Empty
				, 0
				, merchantAccountId);

			var response1 = SessionCache.SessionCache.GetProcessRequest<MerchantAccountActivateResponseData>(request, _requestType);
			var response2 = SessionCache.SessionCache.GetProcessRequest<MerchantAccountActivateResponseData>(request, _requestType);

			Debug.WriteLine(response2.ToXML());
			Assert.IsTrue(response2.IsSuccess);
			Assert.IsTrue(response1.ToXML().Equals(response2.ToXML()));
		}
	}
}
