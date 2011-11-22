using Atlantis.Framework.SEVGetWebsiteId.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.SEVGetWebsiteId.Tests
{
	[TestClass]
	public class GetSEVGetWebsiteIdTests
	{
		private const string _shopperId = "842749";
		private const int _requestType = 457;
	
	
		public GetSEVGetWebsiteIdTests()
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
		public void SEVGetWebsiteIdTest()
		{
			SEVGetWebsiteIdRequestData request = new SEVGetWebsiteIdRequestData(_shopperId
				, string.Empty
				, string.Empty
				, string.Empty
				, 0 );

			SEVGetWebsiteIdResponseData response = (SEVGetWebsiteIdResponseData)Engine.Engine.ProcessRequest(request, _requestType);
			
			Assert.IsTrue(response.IsSuccess);
		}
	}
}
