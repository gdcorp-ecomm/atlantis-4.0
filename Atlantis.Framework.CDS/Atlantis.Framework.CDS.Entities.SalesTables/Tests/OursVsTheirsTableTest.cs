using Atlantis.Framework.CDS.Entities.SalesTables.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestSetUpAndSettings;

namespace Atlantis.Framework.CDS.Entities.SalesTables.Tests
{
  [TestClass()]
  class OursVsTheirsTableTest
  {
    private TestContext testContextInstance;

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

    [TestCategory("CDS"), Priority(0), TestAssertions(3), TestMethod]
    public void OursVsTheirsTableConstructorTest()
    {
      OursVsTheirsTable target = new OursVsTheirsTable();
      Assert.IsNotNull(target, "OursVsTheirsTable is null.");
      Assert.IsNull(target.Columns, "OursVsTheirsTable columns default are not null.");
      Assert.IsNull(target.Rows, "OursVsTheirsTable rows default are not null.");
    }

    [TestCategory("CDS"), Priority(0), TestAssertions(1), TestMethod]
    public void OursVsTheirsTableColumnConstructorTest()
    {
      OursVsTheirsTable.OursVsTheirsTableColumn target = new OursVsTheirsTable.OursVsTheirsTableColumn();
      Assert.IsNotNull(target, "OursVsTheirsTableColumn is null.");
    }

    [TestCategory("CDS"), Priority(0), TestAssertions(1), TestMethod]
    public void OursVsTheirsTableRowConstructorTest()
    {
      OursVsTheirsTable.OursVsTheirsTableRow target = new OursVsTheirsTable.OursVsTheirsTableRow();
      Assert.IsNotNull(target, "OursVsTheirsTableRow is null.");
    }
  }
}
