using System;
using Atlantis.Framework.PrivacyRenewalBasketXml.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PrivacyRenewalBasketXml.Tests
{
  [TestClass]
  public class PrivacyRenewalBasketXmlTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPrivacyRenewalXml()
    {
      // THISSISADEVTESTDOMAINTRWALKER.COM
      int domainBillingResouceId = 375882;
      int privateRegistrationRenewalPfId = 17001;
      int duration = 2;

      PrivacyRenewalBasketXmlRequestData requestData = new PrivacyRenewalBasketXmlRequestData(privateRegistrationRenewalPfId,
                                                                                            PrivacyRenewalBasketXmlRequestData.PrivateRenewalType.Privacy,
                                                                                            duration,
                                                                                            domainBillingResouceId,
                                                                                            "847235",
                                                                                            string.Empty,
                                                                                            string.Empty,
                                                                                            string.Empty,
                                                                                            0);

      PrivacyRenewalBasketXmlResponseData responseData = (PrivacyRenewalBasketXmlResponseData)Engine.Engine.ProcessRequest(requestData, 131);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(!string.IsNullOrEmpty(responseData.RenewalXml));
      Console.WriteLine("CartItemResourceId: " + responseData.CartItemResourceId);
      Console.WriteLine(responseData.RenewalXml);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetBusinessRenewalXml()
    {
      // THISSISADEVTESTDOMAINTRWALKER.COM
      int domainBillingResouceId = 375882;
      int businessRegistrationRenewalPfId = 10084;
      int duration = 2;

      PrivacyRenewalBasketXmlRequestData requestData = new PrivacyRenewalBasketXmlRequestData(businessRegistrationRenewalPfId,
                                                                                            PrivacyRenewalBasketXmlRequestData.PrivateRenewalType.Business,
                                                                                            duration,
                                                                                            domainBillingResouceId,
                                                                                            "847235",
                                                                                            string.Empty,
                                                                                            string.Empty,
                                                                                            string.Empty,
                                                                                            0);

      PrivacyRenewalBasketXmlResponseData responseData = (PrivacyRenewalBasketXmlResponseData)Engine.Engine.ProcessRequest(requestData, 131);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(!string.IsNullOrEmpty(responseData.RenewalXml));
      Console.WriteLine("CartItemResourceId: " + responseData.CartItemResourceId);
      Console.WriteLine(responseData.RenewalXml);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetProtectionRenewalBundleXml()
    {
      // NEWTIMQBCDEVSMART.COM
      int domainBillingResouceId = 376100;
      int protectedRenewalPfId = 776001;
      int duration = 2;

      PrivacyRenewalBasketXmlRequestData requestData = new PrivacyRenewalBasketXmlRequestData(protectedRenewalPfId,
                                                                                            PrivacyRenewalBasketXmlRequestData.PrivateRenewalType.Protection,
                                                                                            duration,
                                                                                            domainBillingResouceId,
                                                                                            "847235",
                                                                                            string.Empty,
                                                                                            string.Empty,
                                                                                            string.Empty,
                                                                                            0);

      PrivacyRenewalBasketXmlResponseData responseData = (PrivacyRenewalBasketXmlResponseData)Engine.Engine.ProcessRequest(requestData, 131);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(!string.IsNullOrEmpty(responseData.RenewalXml));
      Console.WriteLine("CartItemResourceId: " + responseData.CartItemResourceId);
      Console.WriteLine(responseData.RenewalXml);
    }
  }
}
