using System;
using System.Xml;
using Atlantis.Framework.IrisGetServiceMappings.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.IrisGetServiceMappings.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class IrisGetServiceMappingsTests
  {
    public IrisGetServiceMappingsTests()
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

    #region Additional test attributes

    private int _engineRequestId = 339;

    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void TestBlueRazor()
    {
      int resellerId = 2; // BlueRazor
      string shopperId = "855341";
      var request = new IrisGetServiceMappingsRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0,
                                             resellerId);
      var response = (IrisGetServiceMappingsResponseData)Engine.Engine.ProcessRequest(request, _engineRequestId);
      AssertIsStandardXmlResponse(request, response);

      var serviceMappings = response.IrisServiceMappingsResponse;
      Assert.IsNotNull(serviceMappings);
      var globals = serviceMappings.Groupings[IrisServiceMappingGroups.groupGlobal];
      var offerings = serviceMappings.Groupings[IrisServiceMappingGroups.groupOfferings];
      Assert.IsFalse(globals == null && offerings == null);
      Assert.IsTrue(offerings == null ? true : offerings.Count > 0);
      Assert.IsTrue(globals == null ? true : globals.Count > 0);

      foreach (var s in offerings)
      {
        AssertIsValidIrisServiceMapping(s);
      }

      foreach (var s in globals)
      {
        AssertIsValidIrisServiceMapping(s);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void TestGoDaddy()
    {
      int resellerId = 1; // GoDaddy;
      string shopperId = "853516";
      var request = new IrisGetServiceMappingsRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0,
                                             resellerId);
      var response = (IrisGetServiceMappingsResponseData)Engine.Engine.ProcessRequest(request, _engineRequestId);
      AssertIsStandardXmlResponse(request, response);

      var serviceMappings = response.IrisServiceMappingsResponse;
      Assert.IsNotNull(serviceMappings);
      var globals = serviceMappings.Groupings[IrisServiceMappingGroups.groupGlobal];
      var offerings = serviceMappings.Groupings[IrisServiceMappingGroups.groupOfferings];
      Assert.IsFalse(globals == null && offerings == null);
      Assert.IsTrue(offerings == null ? true : offerings.Count > 0);
      Assert.IsTrue(globals == null ? true : globals.Count > 0);

      foreach (var s in offerings)
      {
        AssertIsValidIrisServiceMapping(s);
      }

      foreach (var s in globals)
      {
        AssertIsValidIrisServiceMapping(s);
      }
    }

    private void AssertIsValidIrisServiceMapping(IrisGetServiceMappingsResponseData.IrisServiceMapping mapping)
    {
      Assert.IsFalse(String.IsNullOrEmpty(mapping.FriendlyName));
      Assert.IsFalse(mapping.ServiceId < 0);
    }

    private void AssertIsStandardXmlResponse(IrisGetServiceMappingsRequestData request, IrisGetServiceMappingsResponseData response)
    {
      Assert.IsTrue(response.IsSuccess, "Response returned IsSuccess==false.");

      var xml = new XmlDocument();
      try
      {
        xml.LoadXml(response.Response);
      }
      catch (Exception ex)
      {
        Assert.Fail("Response was not an XML document", ex);
      }

      Assert.IsNotNull(xml.SelectSingleNode("/*"));
    }


  }
}
