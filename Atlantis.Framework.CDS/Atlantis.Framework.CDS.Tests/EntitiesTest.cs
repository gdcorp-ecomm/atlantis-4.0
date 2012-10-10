using System;
using System.Net;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.CDS.Entities.Widgets;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CDS.Tests
{
    /// <summary>
    ///This is a test class for CDS Entities and is intended
    ///to contain all Entities Unit Tests
    ///</summary>
    [TestClass()]
    public class EntitiesTest
    {
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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CDSResponseData Constructor.
        ///</summary>
        [TestMethod()]
        public void CDSJsonContentConstructorTest()
        {
            CDSJsonContent jsContent = new CDSJsonContent("22", "test");
            Assert.AreEqual("test", jsContent.Html,"Html string is null and was not initialized by contructor.");
            Assert.AreEqual("22", jsContent.TargetDivID, "Target div ID is not 22 and was not initialized by contructor.");
        }

        /// <summary>
        ///A test for the disclaimer class.
        ///</summary>
        [TestMethod()]
        public void DisclaimerTest()
        {
            Atlantis.Framework.CDS.Entities.Widgets.Disclaimer.ModalData md = new Atlantis.Framework.CDS.Entities.Widgets.Disclaimer.ModalData();
            Assert.AreEqual(md.Text, "Click here for product disclaimers and legal policies.", "Default disclaimer text was not correct.");
          
        }


    }

}
