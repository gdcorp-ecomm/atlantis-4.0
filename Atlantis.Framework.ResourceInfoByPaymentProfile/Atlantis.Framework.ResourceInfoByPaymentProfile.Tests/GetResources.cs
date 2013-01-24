using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ResourceInfoByPaymentProfile.Interface;

namespace Atlantis.Framework.ResourceInfoByPaymentProfile.Tests
{
  [TestClass]
  public class GetResources
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ResourceInfoByPaymentProfile.Impl.dll")]
    public void GetResourcesTest()
    {
      List<string> _profileList = new List<string>();
      _profileList.Add("58628");
      _profileList.Add("");
      _profileList.Add("59597");

      List<string> _namespaceList = new List<string>();
      _namespaceList.Add("hosting");
      _namespaceList.Add("bundle");

      ResourceInfoByPaymentProfileRequestData request = new ResourceInfoByPaymentProfileRequestData("856907", string.Empty, string.Empty, string.Empty,0);
      
      request.ProfileList = _profileList;
      //request.NameSpaceFilterList = _namespaceList;
      //request.ReturnAll = 1;
      //request.RowsPerPage = 400;

      //request.CheckProfileResourceCountOnly = true; // skips processing row data

      
      ResourceInfoByPaymentProfileResponseData response = (ResourceInfoByPaymentProfileResponseData)Engine.Engine.ProcessRequest(request, 643);
      
      List<ResourceInfo> _resourceInfos = response.GetResourceList;

      int totalPages = response.TotalPages;
      int totalRecords = response.TotalRecords;

      //string xml = response.ToXML();

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
