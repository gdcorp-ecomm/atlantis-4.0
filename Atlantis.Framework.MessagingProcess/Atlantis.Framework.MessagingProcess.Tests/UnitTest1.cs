using System;
using Atlantis.Framework.MessagingProcess.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MessagingProcess.Tests
{
  [TestClass]
  public class UnitTest1
  {

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BasicMessageSend1()
    {
      //string xml = TestEmail1Xml;
      MessagingProcessRequestData request = new MessagingProcessRequestData(
              "840820", string.Empty, string.Empty, string.Empty, 0, 1, "SuperBowlCommercial", "TellAFriend");

      ResourceItem resource = new ResourceItem("SuperBowl", "0");
      resource["EMAIL_MESSAGE"] = new AttributeValue("message");
      resource["FROM_EMAIL"] = new AttributeValue("pmccormack@godaddy.com");

      ContactPointItem contactPoint = new ContactPointItem("FriendContact", "840820");
      contactPoint.ExcludeContactPointType = true;

      contactPoint["firstname"] = "JG";
      contactPoint["lastname"] = string.Empty;
      contactPoint["email"] = "pmccormack@godaddy.com";
      contactPoint["emailtype"] = "html";
      contactPoint["sendemail"] = "true";
      resource.ContactPoints.Add(contactPoint);
      request.AddResource(resource);

      MessagingProcessResponseData response =
              (MessagingProcessResponseData) Engine.Engine.ProcessRequest(request, 66);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ResetPasswordEmail()
    {
      var UserName = "855307";
      //var UserName = "76609"; // test

      MessagingProcessRequestData request = new MessagingProcessRequestData(UserName, string.Empty, string.Empty, string.Empty, 0, 1,
                                                                            "PasswordReset", "MYA");

      //request.AddDictionaryItem("LocalizationCode", "ES");

      ResourceItem resource = new ResourceItem("Shopper", UserName);
      resource["AuthToken"] = new AttributeValue(Guid.NewGuid().ToString());
      resource["Login"] = new AttributeValue(UserName);

      ContactPointItem contactPoint = new ContactPointItem("ShopperContact", "SHOPPER");
      contactPoint["id"] = UserName;
      contactPoint["SendEmail"] = "true";

      resource.ContactPoints.Add(contactPoint);

      request.AddResource(resource);

      MessagingProcessResponseData response =
              (MessagingProcessResponseData) Engine.Engine.ProcessRequest(request, 66);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void WelcomeEmail()
    {
      var UserName = "855307"; // dev
      //var UserName = "76609"; // test

      MessagingProcessRequestData request = new MessagingProcessRequestData(UserName, string.Empty, string.Empty, string.Empty, 0, 1,
                                                                            "AccountCreation", "OrderProcessing");

      request.AddDictionaryItem("LocalizationCode", "ES");

      ResourceItem resource = new ResourceItem("Shopper", UserName);

      ContactPointItem contactPoint = new ContactPointItem("ShopperContact", "SHOPPER");
      contactPoint["id"] = UserName;
      contactPoint["SendEmail"] = "true";

      resource.ContactPoints.Add(contactPoint);

      request.AddResource(resource);

      MessagingProcessResponseData response =
              (MessagingProcessResponseData) Engine.Engine.ProcessRequest(request, 66);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void RetrieveShopperNum()
    {
      var UserName = "855307";
      //var UserName = "76609"; // test

      MessagingProcessRequestData request = new MessagingProcessRequestData(UserName, string.Empty, string.Empty, string.Empty, 0, 1,
                                                                            "LoginInformation", "MYA");

      request.AddDictionaryItem("LocalizationCode", "ES");

      ResourceItem resource = new ResourceItem("Shopper", UserName);
      resource["Login"] = new AttributeValue(UserName);

      ContactPointItem contactPoint = new ContactPointItem("ShopperContact", "SHOPPER");
      contactPoint["id"] = UserName;
      contactPoint["SendEmail"] = "true";

      resource.ContactPoints.Add(contactPoint);

      request.AddResource(resource);

      MessagingProcessResponseData response = (MessagingProcessResponseData) Engine.Engine.ProcessRequest(request, 66);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}