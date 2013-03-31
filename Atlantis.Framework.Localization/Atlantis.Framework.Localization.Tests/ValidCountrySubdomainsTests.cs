using Atlantis.Framework.Localization.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using System.Linq;

namespace Atlantis.Framework.Localization.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  public class ValidCountrySubdomainTests
  {
    private const int _REQUESTTYPE = 673;

    [TestMethod]
    public void RequestDataCacheKeySame()
    {
      ValidCountrySubdomainsRequestData request = new ValidCountrySubdomainsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      ValidCountrySubdomainsRequestData request2 = new ValidCountrySubdomainsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void ResponseDataNullInput()
    {
      ValidCountrySubdomainsResponseData response = ValidCountrySubdomainsResponseData.FromDelimitedSetting(null);
      Assert.AreEqual(0, response.Count);
      Assert.IsNull(response.GetException());
    }

    [TestMethod]
    public void ResponseDataEmptyInput()
    {
      ValidCountrySubdomainsResponseData response = ValidCountrySubdomainsResponseData.FromDelimitedSetting(string.Empty);
      Assert.AreEqual(0, response.Count);
      Assert.IsNull(response.GetException());
    }

    [TestMethod]
    public void ResponseDataIsValidCountrySubdomain()
    {
      string appSettingData = "ZZ|YY";
      ValidCountrySubdomainsResponseData response = ValidCountrySubdomainsResponseData.FromDelimitedSetting(appSettingData);
      Assert.IsTrue(response.IsValidCountrySubdomain("zz"));
      Assert.IsFalse(response.IsValidCountrySubdomain("Yz"));
      Assert.IsFalse(response.IsValidCountrySubdomain("|Y"));
    }

    [TestMethod]
    public void ResponseDataCount()
    {
      string appSettingData = "ZZ|YY";
      ValidCountrySubdomainsResponseData response = ValidCountrySubdomainsResponseData.FromDelimitedSetting(appSettingData);
      Assert.AreEqual(2, response.Count);
    }

    [TestMethod]
    public void ResponseDataXml()
    {
      string appSettingData = "ZZ|YY";
      ValidCountrySubdomainsResponseData response = ValidCountrySubdomainsResponseData.FromDelimitedSetting(appSettingData);
      string xml = response.ToXML();

      XElement element = XElement.Parse(xml);
      Assert.AreEqual(2, element.Descendants("country").Count());
    }

    [TestMethod]
    public void ResponseDataEnumerate()
    {
      string appSettingData = "ZZ|YY";
      ValidCountrySubdomainsResponseData response = ValidCountrySubdomainsResponseData.FromDelimitedSetting(appSettingData);

      int count = 0;

      foreach (string validSubdomain in response.ValidCountrySubdomains)
      {
        count++;
      }

      Assert.AreEqual(2, count);

    }

    [TestMethod]
    public void ResponseDataInputWithEmpties()
    {
      string appSettingData = "ZZ||||YY|";
      ValidCountrySubdomainsResponseData response = ValidCountrySubdomainsResponseData.FromDelimitedSetting(appSettingData);
      Assert.AreEqual(2, response.Count);
    }


    [TestMethod]
    public void RequestExecute()
    {
      ValidCountrySubdomainsRequestData request = new ValidCountrySubdomainsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ValidCountrySubdomainsResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsNotNull(response);
    }

  }
}
