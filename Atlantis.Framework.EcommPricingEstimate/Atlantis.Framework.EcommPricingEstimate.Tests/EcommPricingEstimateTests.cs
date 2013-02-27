using System;
using Atlantis.Framework.EcommPricingEstimate.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommPricingEstimate.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.EcommPricingEstimate.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.EcommPricingEstimate.Interface.dll")]
  public class EcommPricingEstimateTests
  {
    private const string VALID_DEV_SHOPPER = "870808";
    private const string VALID_DEV_SHOPPER_PASSWORD = "passsword";

    [TestMethod]
    public void SEV()
    {
      int PFID = 1401;
      string discount_code = "cbsev01";
      string currency = "USD";
      EcommPricingEstimateRequestData requestData = new EcommPricingEstimateRequestData(VALID_DEV_SHOPPER,
                                                                                        string.Empty,
                                                                                        string.Empty,
                                                                                        string.Empty,
                                                                                        0,
                                                                                        0,
                                                                                        1,
                                                                                        currency,
                                                                                        PFID,
                                                                                        discount_code);
      EcommPricingEstimateResponseData responseData = (EcommPricingEstimateResponseData)Engine.Engine.ProcessRequest(requestData, 656);
      Assert.IsTrue(responseData.IsSuccess);
    }

    [TestMethod]
    public void EmailUS()
    {
      int PFID = 1865;
      string discount_code = "cbeml02";
      string currency = "USD";
      EcommPricingEstimateRequestData requestData = new EcommPricingEstimateRequestData(VALID_DEV_SHOPPER,
                                                                                        string.Empty,
                                                                                        string.Empty,
                                                                                        string.Empty,
                                                                                        0,
                                                                                        0,
                                                                                        1,
                                                                                        currency,
                                                                                        PFID,
                                                                                        discount_code);
      EcommPricingEstimateResponseData responseData = (EcommPricingEstimateResponseData)Engine.Engine.ProcessRequest(requestData, 656);
      Assert.IsTrue(responseData.IsSuccess);
    }

    [TestMethod]
    public void EmailEU()
    {
      int PFID = 6413;
      string discount_code = "cbeml02";
      string currency = "USD";
      EcommPricingEstimateRequestData requestData = new EcommPricingEstimateRequestData(VALID_DEV_SHOPPER,
                                                                                        string.Empty,
                                                                                        string.Empty,
                                                                                        string.Empty,
                                                                                        0,
                                                                                        0,
                                                                                        1,
                                                                                        currency,
                                                                                        PFID,
                                                                                        discount_code);
      EcommPricingEstimateResponseData responseData = (EcommPricingEstimateResponseData)Engine.Engine.ProcessRequest(requestData, 656);
      Assert.IsTrue(responseData.IsSuccess);
    }

    [TestMethod]
    public void EmailAP()
    {
      int PFID = 7413;
      string discount_code = "cbeml02";
      string currency = "USD";
      EcommPricingEstimateRequestData requestData = new EcommPricingEstimateRequestData(VALID_DEV_SHOPPER,
                                                                                        string.Empty,
                                                                                        string.Empty,
                                                                                        string.Empty,
                                                                                        0,
                                                                                        0,
                                                                                        1,
                                                                                        currency,
                                                                                        PFID,
                                                                                        discount_code);
      EcommPricingEstimateResponseData responseData = (EcommPricingEstimateResponseData)Engine.Engine.ProcessRequest(requestData, 656);
      Assert.IsTrue(responseData.IsSuccess);
    }

    [TestMethod]
    public void WSB()
    {
      int PFID = 7524;
      string discount_code = "cbwsb_01";
      string currency = "USD";
      EcommPricingEstimateRequestData requestData = new EcommPricingEstimateRequestData(VALID_DEV_SHOPPER,
                                                                                        string.Empty,
                                                                                        string.Empty,
                                                                                        string.Empty,
                                                                                        0,
                                                                                        0,
                                                                                        1,
                                                                                        currency,
                                                                                        PFID,
                                                                                        discount_code);
      EcommPricingEstimateResponseData responseData = (EcommPricingEstimateResponseData)Engine.Engine.ProcessRequest(requestData, 656);
      Assert.IsTrue(responseData.IsSuccess);
    }

    [TestMethod]
    public void WordPress()
    {
      int PFID = 7524;
      string discount_code = "cbwsb_01";
      string currency = "USD";
      EcommPricingEstimateRequestData requestData = new EcommPricingEstimateRequestData(VALID_DEV_SHOPPER,
                                                                                        string.Empty,
                                                                                        string.Empty,
                                                                                        string.Empty,
                                                                                        0,
                                                                                        0,
                                                                                        1,
                                                                                        currency,
                                                                                        PFID,
                                                                                        discount_code);
      EcommPricingEstimateResponseData responseData = (EcommPricingEstimateResponseData)Engine.Engine.ProcessRequest(requestData, 656);
      Assert.IsTrue(responseData.IsSuccess);
    }
  }
}
