using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Engine;
using Atlantis.Framework.PromoToolGetViralPromo.Interface;

namespace PromoToolGetViralPromoTests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class GetViralPromoTests
	{
		public GetViralPromoTests()
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
		[DeploymentItem("atlantis.confg")]
		public void BasicTest()
		{
			Guid pathway = Guid.NewGuid();
			PromoToolGetViralPromoRequestData request = new PromoToolGetViralPromoRequestData(
        "860427", "http://yuck.com", string.Empty, string.Empty, 0, "599coms");

      for (int i = 0; i < 100; i++)
      {
        PromoToolGetViralPromoResponseData response = (PromoToolGetViralPromoResponseData)Engine.ProcessRequest(request, 600);
        Assert.IsNotNull(response);
      }
		}
	}
}
