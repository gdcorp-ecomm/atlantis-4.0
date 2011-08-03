using System;
using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.OrionGetUsage.Interface;


namespace Atlantis.Framework.OrionGetUsage.Tests
{
  [TestClass]
  public class OrionGetUsageTests
  {
    const string _shopper_id = "840820";
    const string _orionResourceId = "7b233a4b-979b-497e-80ed-5dde71a31b0d"; //"be9297bc-f8e6-49a5-9b95-33db354271f1";    
    const string _usageType = "DISK_SPACE";
    readonly DateTime _startDate = Convert.ToDateTime("07/21/2006");
    readonly DateTime _endDate = Convert.ToDateTime("08/21/2006");
    readonly TimeSpan _timeout = TimeSpan.FromSeconds(30d);
    const int _requestType = 127;

    public OrionGetUsageTests()
    {
    }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void OrionGetUsageTest()
    {
      var request = new OrionGetUsageRequestData(_shopper_id
                                                 , string.Empty
                                                 , string.Empty
                                                 , string.Empty
                                                 , 0
                                                 , _orionResourceId
                                                 , _usageType
                                                 , _startDate
                                                 , _endDate
                                                 , _timeout);

      var response = (OrionGetUsageResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      //This line is needed for test harness only...
      //invoke datacache to properly load dll so call to datacache within OrionGetUsageRequest works
      DataCache.DataCache.GetPrivateLabelType(1);  

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
