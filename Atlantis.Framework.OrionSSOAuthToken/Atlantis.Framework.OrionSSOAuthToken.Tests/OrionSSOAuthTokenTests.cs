using System.Diagnostics;
using Atlantis.Framework.OrionSSOAuthToken.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.OrionSSOAuthToken.Tests
{
	[TestClass]
	public class GetOrionSSOAuthTokenTests
	{
		private const string _shopperId = "856907";
		private const int _requestType = 451;


		public GetOrionSSOAuthTokenTests()
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
		public void OrionSSOAuthTokenTest()
		{
			OrionSSOAuthTokenRequestData request = new OrionSSOAuthTokenRequestData(_shopperId
				, string.Empty
				, string.Empty
				, string.Empty
				, 0
				, 1
				, "sharedhosting");

			OrionSSOAuthTokenResponseData response = (OrionSSOAuthTokenResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

			Debug.WriteLine(response.ToXML());
			Assert.IsTrue(response.IsSuccess);
		}
	}
}
