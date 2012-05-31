using System.Diagnostics;
using Atlantis.Framework.SsoServiceProviderGet.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.SsoServiceProviderGet.Tests
{
  [TestClass]
  public class SsoServiceProviderGetTests
  {
    //private const string _serviceProviderName = "SSO-EDIT-TOOL";
    //private const string _serviceProviderName = "GDMPMSHIP";
    private const string _serviceProviderName = null;
    private const int _requestType = 537;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void SsoServiceProviderGetTest()
    {
      var request = new SsoServiceProviderGetRequestData(string.Empty
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , _serviceProviderName);

      var response = (SsoServiceProviderGetResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      foreach (SsoServiceProviderItem item in response.SsoServiceProviders)
      {
        Debug.WriteLine(string.Format("ServiceProviderName: {0}", item.ServiceProviderName));
        Debug.WriteLine(string.Format("IdentityProviderName: {0}", item.IdentityProviderName));
        Debug.WriteLine(string.Format("ServiceProviderGroupName: {0}", item.ServiceProviderGroupName));
        Debug.WriteLine(string.Format("LoginReceive: {0}", item.LoginReceive));
        Debug.WriteLine(string.Format("LoginReceiveType: {0}", item.LoginReceiveType));
        Debug.WriteLine(string.Format("ServerName: {0}", item.ServerName));
        Debug.WriteLine(string.Format("IsRetired: {0}", item.IsRetired));
        Debug.WriteLine(string.Format("RetiredDate: {0}", item.RetiredDate));
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
