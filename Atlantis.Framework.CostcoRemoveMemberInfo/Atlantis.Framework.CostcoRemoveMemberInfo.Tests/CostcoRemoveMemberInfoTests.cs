using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CostcoRemoveMemberInfo.Interface;

namespace Atlantis.Framework.CostcoRemoveMemberInfo.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class CostcoRemoveMemberInfoTests
  {
    public CostcoRemoveMemberInfoTests()
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

    private int _engineRequestId = 370;
    private string goDaddyShopperId = "863301";

    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestRemoveShopper()
    {
      var request = new CostcoRemoveMemberInfoRequestData(goDaddyShopperId, string.Empty, string.Empty, string.Empty, 0);
      var response = (CostcoRemoveMemberInfoResponseData)Engine.Engine.ProcessRequest(request, _engineRequestId);
      Assert.IsTrue(true);


    }
  }
}
