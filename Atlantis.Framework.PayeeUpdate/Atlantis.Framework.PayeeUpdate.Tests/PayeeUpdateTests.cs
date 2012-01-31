using Atlantis.Framework.PayeeProfileClass.Interface;
using Atlantis.Framework.PayeeProfileGet.Interface;
using Atlantis.Framework.PayeeUpdate.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PayeeUpdate.Tests
{
	[TestClass]
	public class GetPayeeUpdateTests
	{
		private const string _shopperId = "856907";
		private const int _requestType = 479;


		public GetPayeeUpdateTests()
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
		public void PayeeUpdateTest()
		{
			PayeeProfileGetRequestData getRequest = new PayeeProfileGetRequestData(_shopperId
				, string.Empty
				, string.Empty
				, string.Empty
				, 0
				, 1000576);

			PayeeProfileGetResponseData getResponse = (PayeeProfileGetResponseData)Engine.Engine.ProcessRequest(getRequest, 477);
			PayeeProfile originalPayee = getResponse.Profile;
			PayeeProfile updatedPayee = CopyPayee(originalPayee);

			updatedPayee.FriendlyName = "UpdateTest3";
			updatedPayee.PayPalEmail = "ksearle@godaddy.com";

			PayeeUpdateRequestData request = new PayeeUpdateRequestData(_shopperId
				, string.Empty
				, string.Empty
				, string.Empty
				, 0
				, originalPayee
				, updatedPayee);

			PayeeUpdateResponseData response = (PayeeUpdateResponseData)Engine.Engine.ProcessRequest(request, _requestType);

			Assert.IsTrue(response.IsSuccess);
		}

		private PayeeProfile CopyPayee (PayeeProfile originalPayee)
		{
			PayeeProfile newPayee = new PayeeProfile();

			foreach (string key in originalPayee.Keys)
			{
				newPayee[key] = originalPayee[key];
			}

			foreach (PayeeProfile.ACHClass achItem in originalPayee.ACH)
			{
				newPayee.ACH.Add(achItem);
			}

			foreach (PayeeProfile.AddressClass address in originalPayee.Address)
			{
				newPayee.Address.Add(address);
			}

			return newPayee;
		}
	}
}
