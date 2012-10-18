using System;
using System.Net;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.CDS.Entities.Homepage.Widgets;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestSetUpAndSettings;

namespace Atlantis.Framework.CDS.Tests
{
    /// <summary>
    ///This is a test class for CDS Entities - BannerInfo And BannerPod 
    ///</summary>
    [TestClass()]
    public class BulkPanelTest
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
        ///A test for the BulkPanel constructor.
        ///</summary>
        [TestCategory("CDS"), Priority(0), TestAssertions(2), TestMethod]
        public void BulkPanelConstructorTest()
        {
          HPBulkPanel target = new HPBulkPanel();
            Assert.IsNotNull(target, "BulkPanel is null.");
            Assert.IsNotNull(target.BulkLinkItems, "BulkLinkItems is null");
        }

        /// <summary>
        ///A test for BulkPanel properties.
        ///</summary>
        [TestCategory("CDS"), Priority(0), TestAssertions(8), TestMethod]
        public void BulkPanelPropertiesTest()
        {
          HPBulkPanel target = new HPBulkPanel();

            string expectedResult = "broncosBlue";
            target.BackgroundTopColor = expectedResult;
            Assert.AreEqual(expectedResult, target.BackgroundTopColor, "BulkPanel.BackgroundTopColor was not set correctly.");

            expectedResult = "broncosOrange";
            target.BackgroundBottomColor = expectedResult;
            Assert.AreEqual(expectedResult, target.BackgroundBottomColor, "BulkPanel.BackgroundBottomColor was not set correctly.");

            expectedResult = "Here is my header text";
            target.HeaderText = expectedResult;
            Assert.AreEqual(expectedResult, target.HeaderText, "BulkPanel.HeaderText was not set correctly.");

            expectedResult = "Here is my wait message text test";
            target.WaitMessage = expectedResult;
            Assert.AreEqual(expectedResult, target.WaitMessage, "BulkPanel.WaitMessage was not set correctly.");

            expectedResult = "Here is my PromoDisclaimerText";
            target.PromoDisclaimerText = expectedResult;
            Assert.AreEqual(expectedResult, target.PromoDisclaimerText, "BulkPanel.PromoDisclaimerText was not set correctly.");

            expectedResult = "Here is my TeaserMessage text";
            target.TeaserMessage = expectedResult;
            Assert.AreEqual(expectedResult, target.TeaserMessage, "BulkPanel.TeaserMessage was not set correctly.");

            expectedResult = "Here is my SearchBoxText text";
            target.SearchBoxText = expectedResult;
            Assert.AreEqual(expectedResult, target.SearchBoxText, "BulkPanel.SearchBoxText was not set correctly.");

            expectedResult = "Here is my TldsOffered .com, .info... etc";
            target.TldsOffered = expectedResult;
            Assert.AreEqual(expectedResult, target.TldsOffered, "BulkPanel.TldsOffered was not set correctly.");

        }

        /// <summary>
        ///A test for BulkLinkItem constructor.
        ///</summary>
        [TestCategory("CDS"), Priority(0), TestAssertions(1), TestMethod]
        public void BulkLinkItemConstructorTest()
        {
          HPBulkLinkItem target = new HPBulkLinkItem();
            Assert.IsNotNull(target, "BulkLinkItem is null.");
        }

        /// <summary>
        ///A test for BannerInfo properties.
        ///</summary>
        [TestCategory("CDS"), Priority(0), TestAssertions(3), TestMethod]
        public void BulkLinkItemPropertiesTest()
        {
          HPBulkLinkItem target = new HPBulkLinkItem();

            string expectedResult = "right";
            target.LinkPosition = expectedResult;
            Assert.AreEqual(expectedResult, target.LinkPosition, "BulkLinkItem.LinkPosition was not set correctly.");

            expectedResult = "Here is my link text";
            target.LinkText = expectedResult;
            Assert.AreEqual(expectedResult, target.LinkText, "BulkLinkItem.LinkText was not set correctly.");

            expectedResult = "Here is my link";
            target.Link = expectedResult;
            Assert.AreEqual(expectedResult, target.Link, "BulkLinkItem.Link was not set correctly.");

        }



    }

}
