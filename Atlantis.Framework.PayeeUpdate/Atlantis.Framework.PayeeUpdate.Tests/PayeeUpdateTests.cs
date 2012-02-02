﻿using Atlantis.Framework.PayeeProfileClass.Interface;
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
				, 1000579);

			PayeeProfileGetResponseData getResponse = (PayeeProfileGetResponseData)Engine.Engine.ProcessRequest(getRequest, 477);
			PayeeProfile originalPayee = getResponse.Profile;
			PayeeProfile updatedPayee = CopyPayee(originalPayee);

			//updatedPayee.FriendlyName = "UpdateTest4";
			//updatedPayee.PayPal.Email = "asearle@godaddy.com";
			updatedPayee.ACH.AchBankName = "Kent Searle";
			updatedPayee.Address[0].Country = "US";
			updatedPayee.ACH.AchRTN = "155487006";
			updatedPayee.ACH.AccountNumber = "8227444441";

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

			if (originalPayee.ACH.Count > 0)
			{
				foreach (string key in originalPayee.ACH.Keys)
				{
					newPayee.ACH[key] = originalPayee.ACH[key];
				}
			}

			if (originalPayee.PayPal.Count > 0)
			{
				foreach (string key in originalPayee.PayPal.Keys)
				{
					newPayee.PayPal[key] = originalPayee.PayPal[key];
				}
			}

			if (originalPayee.GAG.Count > 0)
			{
				foreach (string key in originalPayee.GAG.Keys)
				{
					newPayee.GAG[key] = originalPayee.GAG[key];
				}
			}

			foreach (PayeeProfile.AddressClass address in originalPayee.Address)
			{
				newPayee.Address.Add(address);
			}

			return newPayee;
		}
	}
}
