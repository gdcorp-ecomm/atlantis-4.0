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
    public class GeneralEntitiesTest
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
            Atlantis.Framework.CDS.Entities.Widgets.Disclaimer md = new Atlantis.Framework.CDS.Entities.Widgets.Disclaimer();
            Assert.AreEqual(md.CurrentModal.Text, "Click here for product disclaimers and legal policies.", "Default disclaimer text was not correct.");

        }
        
          /// <summary>
        ///A test for the DppFreeExtras class column number.
        ///</summary>
        [TestMethod()]
        public void DppFreeExtrasColumnNumberTest()
        {
            Atlantis.Framework.CDS.Entities.Widgets.DPPFreeExtras target = new Atlantis.Framework.CDS.Entities.Widgets.DPPFreeExtras();
            Assert.AreEqual(target.NumColumns, 2, "Column number is not 2 and it should be.");
        }

        /// <summary>
        ///A test for List class column number.
        ///</summary>
        [TestMethod()]
        public void ListColumnNumberTest()
        {
            Atlantis.Framework.CDS.Entities.Widgets.List target = new Atlantis.Framework.CDS.Entities.Widgets.List();
            Assert.AreEqual(target.NumColumns, 2, "Column number is not 2 and it should be.");
        }

        /// <summary>
        ///A test for the ManageNow class link text and description.
        ///</summary>
        [TestMethod()]
        public void ManageNowTest()
        {
            Atlantis.Framework.CDS.Entities.Widgets.ManageNow target = new Atlantis.Framework.CDS.Entities.Widgets.ManageNow();
            Assert.AreEqual(target.LinkText, "Manage Now", "Link text not correct for the ManageNow class.");
            Assert.AreEqual(target.Description, "Already own this product?", "Description text not correct for the ManageNow class.");
        }

        /// <summary>
        ///A test for the Support class title and community group text.
        ///</summary>
        [TestMethod()]
        public void SupportTest()
        {
            Atlantis.Framework.CDS.Entities.Widgets.Support target = new Atlantis.Framework.CDS.Entities.Widgets.Support();
            Assert.AreEqual(target.Title, "Support", "Title text not correct for Support class.");
            Assert.AreEqual(target.CommunityGroup, "product group of your choice", "CommunityGroup text not correct for the Support class.");
        }

     

    }

}
