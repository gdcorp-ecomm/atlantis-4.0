using System.Diagnostics;
using Atlantis.Framework.SsoProviderGroupGet.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.SsoProviderGroupGet.Tests
{
  [TestClass]
  public class SsoProviderGroupGetTests
  {
    //private const string _serviceProviderGroupName = "BRIDP";
    private const string _serviceProviderGroupName = null;
    private const int _requestType = 535;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void SsoProviderGroupGetTest()
    {
      var request = new SsoProviderGroupGetRequestData(string.Empty
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , _serviceProviderGroupName);

      var response = (SsoProviderGroupGetResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      foreach (ServiceProviderGroupItem item in response.ServiceProviderGroups)
      {
        Debug.WriteLine(string.Format("ServiceProviderGroupName: {0}", item.ServiceProviderGroupName));
        Debug.WriteLine(string.Format("RedirectLoginUrl: {0}", item.RedirectLoginUrl));
        Debug.WriteLine(string.Format("LogoutUrl: {0}", item.LogoutUrl));
        Debug.WriteLine(string.Format("RedirectLogoutUrl: {0}", item.RedirectLogoutUrl));
        Debug.WriteLine(string.Format("CreateDate: {0}", item.CreateDate));
        Debug.WriteLine(string.Format("ChangedBy: {0}", item.ChangedBy));
        Debug.WriteLine(string.Format("ApprovedBy: {0}", item.ApprovedBy));
        Debug.WriteLine(string.Format("ActionDescription: {0}", item.ActionDescription));
        Debug.WriteLine("******************************");
      }

      Assert.IsTrue(response.IsSuccess);
    }
  }
}