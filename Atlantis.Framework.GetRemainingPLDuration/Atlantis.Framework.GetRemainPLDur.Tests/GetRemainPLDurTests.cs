using System.Diagnostics;
using Atlantis.Framework.GetRemainPLDur.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GetRemainPLDur.Tests
{
  [TestClass]
  public class GetRemainPLDurTests
  {
    private const string _shopperId = "850659";
    private const int _requestType = 438;

    public GetRemainPLDurTests()
    {
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

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void MYAResellerUpgradesTest()
    {
      const int entityId = 440828;
      GetRemainPLDurRequestData request = new GetRemainPLDurRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , entityId);

      GetRemainPLDurResponseData response = (GetRemainPLDurResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      
      Debug.WriteLine("*********************************");
      Debug.WriteLine(string.Format("Duration: {0}", response.Duration));
      Debug.WriteLine("*********************************");
      Debug.WriteLine(string.Empty);
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
