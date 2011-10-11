using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.DCCIsDomainAlertCancellable.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.DCCIsDomainAlertCancellable.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetDCCIsDomainAlertCancellableTests
  {
    private const string _shopperId = "83439";   //DEV: 856907  TEST: 83439
    private const int _requestType = 224;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCIsDomainAlertCancellableTest()
    {
      List<int> billingResourceIds = new List<int>();
      billingResourceIds.Add(12291);
      billingResourceIds.Add(17665);
      billingResourceIds.Add(12271);
      billingResourceIds.Add(17666);

      DCCIsDomainAlertCancellableRequestData request = new DCCIsDomainAlertCancellableRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "MYA"
         , billingResourceIds);

      DCCIsDomainAlertCancellableResponseData response = (DCCIsDomainAlertCancellableResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
