using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.WhoIsGetInfo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.WhoIsGetInfo.Tests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class WhoIsGetInfoTests
	{
		private const int _requestType = 202;
		private const string _shopperId = "858421"; //"840820";  //840820 842749; 

		public WhoIsGetInfoTests(){}

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
		public void RunWhoIsTest()
		{
		WhoIsGetInfoRequestData request = new WhoIsGetInfoRequestData(_shopperId,
                                                                  string.Empty,
                                                                  string.Empty,
                                                                  string.Empty,
                                                                  0,
																																	"hatdragon.com",
																																	1);
			request.RequestTimeout = new TimeSpan(0,0,0, 10);


		WhoIsGetInfoResponseData response = (WhoIsGetInfoResponseData)Engine.Engine.ProcessRequest(request, _requestType);
			Debug.WriteLine("*************************");
			Debug.WriteLine(string.Format("Requested Domain Name: {0}", response.whoIsInfo.Name));
			Debug.WriteLine("*************************");
			Debug.WriteLine(response.ToXML());
			Assert.IsTrue(response.IsSuccess);
		}

		}
	}
