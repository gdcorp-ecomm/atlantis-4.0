using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.CDS.Entities.Radios.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestSetUpAndSettings;

namespace Atlantis.Framework.CDS.Entities.Radios.Tests
{
  [TestClass()]
  public class RadioContentTest
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

    [TestCategory("CDS"), Priority(0), TestAssertions(2), TestMethod]
    public void RadioContentConstructor()
    {
      RadioContent target = new RadioContent();
      Assert.IsNotNull(target, "RadioContent is null.");
      Assert.IsNotNull(target, "RadioContent CurrentRadio is null.");
    }

    [TestCategory("CDS"), Priority(0), TestAssertions(1), TestMethod]
    public void CurrentRadioConstructor()
    {
      RadioContent.CurrentRadio target = new RadioContent.CurrentRadio();
      Assert.IsNotNull(target, "RadioContent CurrentRadio is null.");
    }
  }
}
