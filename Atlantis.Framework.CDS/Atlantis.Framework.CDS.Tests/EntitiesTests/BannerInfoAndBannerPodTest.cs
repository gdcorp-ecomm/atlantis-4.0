using System;
using System.Net;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.CDS.Entities.Widgets;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CDS.Tests
{
    /// <summary>
    ///This is a test class for CDS Entities - BannerInfo And BannerPod 
    ///</summary>
    [TestClass()]
    public class BannerInfoAndBannerPodTest
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
        ///A test for the BannerPod Constructor.
        ///</summary>
        [TestMethod()]
        public void BannerPodConstructorTest()
        {
            BannerPod target = new BannerPod();
            Assert.IsNotNull(target, "BannerPod is null.");
            Assert.IsNotNull(target.BannerInfoList, "banner info list is null");
        }

        /// <summary>
        ///A test for BannerInfo Constructor.
        ///</summary>
        [TestMethod()]
        public void BannerInfoConstructorTest()
        {
            BannerInfo target = new BannerInfo();           
            Assert.IsNotNull(target, "BannerInfo is null.");
        }
        
        /// <summary>
        ///A test for BannerInfo properties.
        ///</summary>
        [TestMethod()]
        public void BannerInfoPropertiesTest()
        {
            BannerInfo target = new BannerInfo();

            string expectedResult = "small";
            target.Size = expectedResult;
            Assert.AreEqual(expectedResult, target.Size, "BannerInfo.Size was not set correctly.");

            expectedResult = "blue";
            target.BackgroundColor = expectedResult;
            Assert.AreEqual(expectedResult, target.BackgroundColor, "BannerInfo.BackgroundColor was not set correctly.");

            expectedResult = "testImage";
            target.IconImage = expectedResult;
            Assert.AreEqual(expectedResult, target.IconImage, "BannerInfo.IconImage was not set correctly.");

            expectedResult = "onRight";
            target.ImagePosition = expectedResult;
            Assert.AreEqual(expectedResult, target.ImagePosition, "BannerInfo.ImagePosition was not set correctly.");

            expectedResult = "TestTitle";
            target.Title = expectedResult;
            Assert.AreEqual(expectedResult, target.Title, "BannerInfo.Title was not set correctly.");

            expectedResult = "TestText";
            target.Text = expectedResult;
            Assert.AreEqual(expectedResult, target.Text, "BannerInfo.Text was not set correctly.");

            expectedResult = "ToLeft";
            target.TextAlign = expectedResult;
            Assert.AreEqual(expectedResult, target.TextAlign, "BannerInfo.TextAlign was not set correctly.");

            expectedResult = "VeryBig";
            target.TextSize = expectedResult;
            Assert.AreEqual(expectedResult, target.TextSize, "BannerInfo.TextSize was not set correctly.");

            expectedResult = "BigListOfCountriesHere";
            target.CountryCodesList = expectedResult;
            Assert.AreEqual(expectedResult, target.CountryCodesList, "BannerInfo.CountryCodesList was not set correctly.");

            expectedResult = "121";
            target.SplitValue = expectedResult;
            Assert.AreEqual(expectedResult, target.SplitValue, "BannerInfo.SplitValue was not set correctly.");
        }

     

    }

}
