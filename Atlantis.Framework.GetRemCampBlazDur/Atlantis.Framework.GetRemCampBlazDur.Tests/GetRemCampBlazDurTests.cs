using System.Diagnostics;
using Atlantis.Framework.GetRemCampBlazDur.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GetRemCampBlazDur.Tests
{
  [TestClass]
  public class GetRemCampBlazDurTests
  {
    private const string _shopperId = "850659";
    private const int _requestType = 468;

    public GetRemCampBlazDurTests()
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
    public void MYAEEMUpgradesTest()
    {
      const int entityId = 5079;
      GetRemCampBlazDurRequestData request = new GetRemCampBlazDurRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , entityId);

      GetRemCampBlazDurResponseData response = (GetRemCampBlazDurResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine("*********************************");
      Debug.WriteLine(string.Format("Duration: {0}", response.Duration));
      Debug.WriteLine("*********************************");
      Debug.WriteLine(string.Empty);
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
