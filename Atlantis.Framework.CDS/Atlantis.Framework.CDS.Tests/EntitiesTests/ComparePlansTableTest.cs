using Atlantis.Framework.CDS.Entities.SalesTables.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestSetUpAndSettings;

namespace Atlantis.Framework.CDS.Tests
{
  [TestClass()]
  public class ComparePlansTableTest
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
    public void ComparePlansTableConstructorTest()
    {
      ComparePlansTable target = new ComparePlansTable();
      Assert.IsNotNull(target, "ComparePlansTable is null.");
      Assert.IsNull(target.Columns, "ComparePlansTable columns default are not null.");
      Assert.IsNull(target.Rows, "ComparePlansTable rows default are not null.");
    }

    [TestCategory("CDS"), Priority(0), TestAssertions(1), TestMethod]
    public void ComparePlansTableColumnConstructorTest()
    {
      ComparePlansTable.ComparePlansTableColumn target = new ComparePlansTable.ComparePlansTableColumn();
      Assert.IsNotNull(target, "ComparePlansTableColumn is null.");
    }

    [TestCategory("CDS"), Priority(0), TestAssertions(1), TestMethod]
    public void ComparePlansTableRowConstructorTest()
    {
      ComparePlansTable.ComparePlansTableRow target = new ComparePlansTable.ComparePlansTableRow();
      Assert.IsNotNull(target, "ComparePlansTableRow is null.");
    }
  }
}
