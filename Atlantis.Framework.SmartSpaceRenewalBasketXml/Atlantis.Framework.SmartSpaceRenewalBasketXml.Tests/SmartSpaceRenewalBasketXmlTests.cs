using System;
using Atlantis.Framework.SmartSpaceRenewalBasketXml.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.SmartSpaceRenewalBasketXml.Tests
{
  [TestClass]
  public class SmartSpaceRenewalBasketXmlTests
  {
    [TestMethod]
    public void GetDomainWithSmartSpaceRenewalBasketXml()
    {
      // TESTINDEVOFALLPRODUCTS.COM
      int domainBillingResouceId = 376140;
      int smartSpace2YearRenewalPfid = 16578;
      int duration = 1;

      SmartSpaceRenewalBasketXmlRequestData requestData = new SmartSpaceRenewalBasketXmlRequestData(domainBillingResouceId,
                                                                                                    SmartSpaceRenewalBasketXmlRequestData.BillingResourceIdType.Domain,
                                                                                                    "TESTINDEVOFALLPRODUCTS",
                                                                                                    "COM",
                                                                                                    smartSpace2YearRenewalPfid,
                                                                                                    duration,
                                                                                                    "847235",
                                                                                                    string.Empty,
                                                                                                    string.Empty,
                                                                                                    string.Empty,
                                                                                                    0);

      SmartSpaceRenewalBasketXmlResponseData responseData = (SmartSpaceRenewalBasketXmlResponseData)Engine.Engine.ProcessRequest(requestData, 138);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(!string.IsNullOrEmpty(responseData.RenewalXml));
      Console.WriteLine("CartItemResourceId: " + responseData.CartItemResourceId);
      Console.WriteLine(responseData.RenewalXml);
    }

    [TestMethod]
    public void GetSmartSpaceRenewalBasketXmlWithSmartSpaceBillingResourceId()
    {

      // NEWTIMQBCDEVSMART.COM
      int smartSpaceBillingResourceId = 375882;
      int smartSpace2YearRenewalPfid = 16578;
      int duration = 1;

      SmartSpaceRenewalBasketXmlRequestData requestData = new SmartSpaceRenewalBasketXmlRequestData(smartSpaceBillingResourceId,
                                                                                                    SmartSpaceRenewalBasketXmlRequestData.BillingResourceIdType.SmartSpace,
                                                                                                    "NEWTIMQBCDEVSMART",
                                                                                                    "COM",
                                                                                                    smartSpace2YearRenewalPfid,
                                                                                                    duration,
                                                                                                    "847235",
                                                                                                    string.Empty,
                                                                                                    string.Empty,
                                                                                                    string.Empty,
                                                                                                    0);

      SmartSpaceRenewalBasketXmlResponseData responseData = (SmartSpaceRenewalBasketXmlResponseData)Engine.Engine.ProcessRequest(requestData, 138);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(!string.IsNullOrEmpty(responseData.RenewalXml));
      Console.WriteLine("CartItemResourceId: " + responseData.CartItemResourceId);
      Console.WriteLine(responseData.RenewalXml);
    }

    [TestMethod]
    public void GetDomainWithoutSmartSpaceRenewalBasketXml()
    {
      
      // THISSISADEVTESTDOMAINTRWALKER.COM
      int domainBillingResouceId = 375882;
      int smartSpace2YearRenewalPfid = 16578;
      int duration = 1;

      SmartSpaceRenewalBasketXmlRequestData requestData = new SmartSpaceRenewalBasketXmlRequestData(domainBillingResouceId,
                                                                                                    SmartSpaceRenewalBasketXmlRequestData.BillingResourceIdType.Domain,
                                                                                                    "TESTINDEVOFALLPRODUCTS",
                                                                                                    "COM",
                                                                                                    smartSpace2YearRenewalPfid,
                                                                                                    duration,
                                                                                                    "847235",
                                                                                                    string.Empty,
                                                                                                    string.Empty,
                                                                                                    string.Empty,
                                                                                                    0);

      SmartSpaceRenewalBasketXmlResponseData responseData = (SmartSpaceRenewalBasketXmlResponseData)Engine.Engine.ProcessRequest(requestData, 138);

      Assert.IsFalse(responseData.IsSuccess);
      Console.WriteLine("CartItemResourceId: " + responseData.CartItemResourceId);
      Console.WriteLine(responseData.RenewalXml);
    }
  }
}
