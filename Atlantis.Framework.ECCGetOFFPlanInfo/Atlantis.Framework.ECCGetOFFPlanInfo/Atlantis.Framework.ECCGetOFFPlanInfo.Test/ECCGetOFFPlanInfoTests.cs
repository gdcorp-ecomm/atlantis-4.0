using System;
using System.Diagnostics;
using Atlantis.Framework.Ecc.Interface.Enums;
using Atlantis.Framework.ECCGetOFFPlanInfo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.ECCGetOFFPlanInfo.Test
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class GetECCGetOFFPlanInfoTests
	{
	
		private const string _shopperId = "";
	
	
		public GetECCGetOFFPlanInfoTests()
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
		public void ECCGetOFFPlanInfoTest()
		{
			string shopperId = "858421";
			int requestType = 290;
			ECCGetOFFPlanInfoResponseData responseData = null;

			ECCGetOFFPlanInfoRequestData requestData = new ECCGetOFFPlanInfoRequestData(shopperId
																																									, string.Empty
																																									, string.Empty
																																									, string.Empty
																																									, 1
																																									, 1
																																									, string.Empty
																																									, string.Empty
																																									, EccOFFPlanInfoRequestStatus.All);

			try
			{

				responseData = (ECCGetOFFPlanInfoResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

			
				if (!responseData.IsSuccess)
				{
					Assert.Fail("Call was not successful.");
				}
				else
				{
					Assert.IsNotNull(responseData.ToString());
					Debug.WriteLine(responseData.ToXML());
				}
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}

			Debug.WriteLine(responseData.ToXML());
			Assert.IsTrue(responseData.IsSuccess);
		}
	}
}
