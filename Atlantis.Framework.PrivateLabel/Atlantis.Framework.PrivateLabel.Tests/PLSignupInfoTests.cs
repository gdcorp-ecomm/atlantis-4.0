using Atlantis.Framework.PrivateLabel.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  public class PLSignupInfoTests
  {
    [TestMethod]
    public void RequestDataProperties()
    {
      var request = new PLSignupInfoRequestData(44);
      Assert.AreEqual(44, request.PrivateLabelId);
    }

    [TestMethod]
    public void RequestDataCacheKey()
    {
      var request = new PLSignupInfoRequestData(44);
      var request2 = new PLSignupInfoRequestData(44);
      var request3 = new PLSignupInfoRequestData(46);

      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
      Assert.AreNotEqual(request.GetCacheMD5(), request3.GetCacheMD5());
    }

    const string _DATAXML = "<item EntityID=\"1724\" CompanyName=\"Blue\" isMCPReseller=\"0\" defaultTransactionCurrencyType=\"CAD\" pricingManagementCurrencyType=\"JPY\"/>";
    const string _DATAXMLEXTRANODE = "<node><item EntityID=\"1724\" CompanyName=\"Blue\" isMCPReseller=\"1\" defaultTransactionCurrencyType=\"CAD\" pricingManagementCurrencyType=\"JPY\"/></node>";

    [TestMethod]
    public void ResponseDataProperties()
    {
      var response = PLSignupInfoResponseData.FromCacheXml(_DATAXML);
      Assert.AreEqual("CAD", response.DefaultTransactionCurrencyType);
      Assert.AreEqual(1724, response.PrivateLabelId);
      Assert.AreEqual("JPY", response.PricingManagementCurrencyType);
      Assert.IsFalse(response.IsMultiCurrencyReseller);

      XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void ResponseDataPropertiesExtraNode()
    {
      var response = PLSignupInfoResponseData.FromCacheXml(_DATAXMLEXTRANODE);
      Assert.AreEqual("CAD", response.DefaultTransactionCurrencyType);
      Assert.AreEqual(1724, response.PrivateLabelId);
      Assert.AreEqual("JPY", response.PricingManagementCurrencyType);
      Assert.IsTrue(response.IsMultiCurrencyReseller);
    }

    [TestMethod]
    public void ResponseDataPropertiesNoItem()
    {
      var response = PLSignupInfoResponseData.FromCacheXml("<data />");
      Assert.AreEqual("USD", response.DefaultTransactionCurrencyType);
      Assert.AreEqual(0, response.PrivateLabelId);
      Assert.IsNull(response.PricingManagementCurrencyType);
      Assert.IsFalse(response.IsMultiCurrencyReseller);
      Assert.IsTrue(ReferenceEquals(response, PLSignupInfoResponseData.Default));
    }

    [TestMethod]
    public void ResponseDataPropertiesNull()
    {
      var response = PLSignupInfoResponseData.FromCacheXml(null);
      Assert.IsTrue(ReferenceEquals(response, PLSignupInfoResponseData.Default));
    }

    [TestMethod]
    public void ResponseDataPropertiesEmpty()
    {
      var response = PLSignupInfoResponseData.FromCacheXml(string.Empty);
      Assert.IsTrue(ReferenceEquals(response, PLSignupInfoResponseData.Default));
    }

    const int _REQUESTTYPE = 522;

    [TestMethod]
    public void BasicSignupInfoValidPrivateLabelId()
    {
      PLSignupInfoRequestData request = new PLSignupInfoRequestData(1724);
      PLSignupInfoResponseData response = (PLSignupInfoResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(ReferenceEquals(response, PLSignupInfoResponseData.Default));
    }

    [TestMethod]
    public void BasicSignupBadPrivateLabelId()
    {
      var request = new PLSignupInfoRequestData(-483);
      PLSignupInfoResponseData response = (PLSignupInfoResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(ReferenceEquals(response, PLSignupInfoResponseData.Default));
    }

  }
}
