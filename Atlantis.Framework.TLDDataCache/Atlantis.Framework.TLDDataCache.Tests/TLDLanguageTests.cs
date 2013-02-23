using Atlantis.Framework.TLDDataCache.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.TLDDataCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class TLDLanguageTests
  {
    const int _TLDLANGUAGEREQUEST = 655;

    [TestMethod]
    public void RequestToXmlForOrg()
    {
      var request = new TLDLanguageRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 3);
      Assert.IsTrue(!string.IsNullOrEmpty(request.ToXML()));
    }

    [TestMethod]
    public void GetRegistryLanguagesForOrg()
    {
      var request = new TLDLanguageRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 3);
      var response = (TLDLanguageResponseData)DataCache.DataCache.GetProcessRequest(request, _TLDLANGUAGEREQUEST);
      Assert.IsTrue(response.RegistryLanguages.Any());
    }

    [TestMethod]
    public void GetRegistryLanguagesForInvalid()
    {
      var request = new TLDLanguageRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 78978797);
      var response = (TLDLanguageResponseData)DataCache.DataCache.GetProcessRequest(request, _TLDLANGUAGEREQUEST);
      Assert.IsTrue(!response.RegistryLanguages.Any());
    }

    [TestMethod]
    public void GetRegistryLanguageByName()
    {
      var request = new TLDLanguageRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 3);
      var response = (TLDLanguageResponseData)DataCache.DataCache.GetProcessRequest(request, _TLDLANGUAGEREQUEST);
      Assert.IsTrue(response.GetLanguageDataByName("belarusian") != null);
    }

    [TestMethod]
    public void GetRegistryLanguageById()
    {
      var request = new TLDLanguageRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 3);
      var response = (TLDLanguageResponseData)DataCache.DataCache.GetProcessRequest(request, _TLDLANGUAGEREQUEST);
      Assert.IsTrue(response.GetLanguageDataById(16) != null);
    }
  }
}
