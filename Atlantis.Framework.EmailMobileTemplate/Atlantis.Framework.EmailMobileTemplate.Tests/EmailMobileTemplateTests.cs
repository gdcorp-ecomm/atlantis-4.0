using System;
using Atlantis.Framework.EmailMobileTemplate.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EmailMobileTemplate.Tests
{
  [TestClass]
  public class EmailMobileTemplateTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetValidMobileEmailTemplate()
    {
      EmailMobileTemplateRequestData requestData = new EmailMobileTemplateRequestData(350235,
                                                                                      "03dfca0b-791a-40a0-8aaf-ec638bddb804",
                                                                                      "gabba01",
                                                                                      "860613",
                                                                                      1,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      0);
      try
      {
        EmailMobileTemplateResponseData responseData = (EmailMobileTemplateResponseData) Engine.Engine.ProcessRequest(requestData, 248);
        if(responseData.IsSuccess)
        {
          Console.WriteLine(responseData.MobileTemplateHtml);
          Assert.IsTrue(!string.IsNullOrEmpty(responseData.MobileTemplateHtml));
        }
        else
        {
          Assert.Fail("Error getting template html.");
        }
      }
      catch(Exception ex)
      {
        Assert.Fail(ex.Message);
      }

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetInValidMobileEmailTemplate()
    {
      EmailMobileTemplateRequestData requestData = new EmailMobileTemplateRequestData(1,
                                                                                      "d0a95a49-1de3-42f0-96cb-362fe6d033d",
                                                                                      "gabba01",
                                                                                      "840820",
                                                                                      1,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      0);
      try
      {
        EmailMobileTemplateResponseData responseData = (EmailMobileTemplateResponseData)Engine.Engine.ProcessRequest(requestData, 248);
        if (responseData.IsSuccess)
        {
          Console.WriteLine(responseData.MobileTemplateHtml);
          Assert.IsTrue(string.IsNullOrEmpty(responseData.MobileTemplateHtml));
        }
        else
        {
          Assert.Fail("Error getting template html.");
        }
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
