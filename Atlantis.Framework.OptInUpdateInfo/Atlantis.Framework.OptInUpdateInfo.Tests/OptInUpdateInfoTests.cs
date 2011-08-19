using System;
using System.Collections.Generic;
using Atlantis.Framework.OptIn.Interface.Enums;
using Atlantis.Framework.OptInUpdateInfo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.OptInUpdateInfo.Tests
{
  [TestClass]
  public class OptInUpdateInfoTests
  {
    private const string SHOPPER_1 = "862210";
    private const string SHOPPER_2 = "847235";

    [TestMethod]
    [DeploymentItem("App.config")]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("DataProvider.xml")]
    public void OneShopperUpdateAllTurnedOff()
    {
      List<OptInPublicationTypes> requestedOptIns = new List<OptInPublicationTypes>();
      requestedOptIns.Add(OptInPublicationTypes.BobsBlog);
      requestedOptIns.Add(OptInPublicationTypes.BusinessOffers);
      requestedOptIns.Add(OptInPublicationTypes.Entertainment);
      requestedOptIns.Add(OptInPublicationTypes.MonthlyStatement);
      requestedOptIns.Add(OptInPublicationTypes.NonPromotional);
      requestedOptIns.Add(OptInPublicationTypes.PostalCommunications);
      requestedOptIns.Add(OptInPublicationTypes.RelatedOffers);
      requestedOptIns.Add(OptInPublicationTypes.SmsCommunications);

      String emailAddress = "kklink@godaddy.com";
      String shopperId = "858421";
      int requestId = 315;

      bool isReseller = false;
      string lastName = "Klink";
      string firstName = "Kris";
      //string userHostAddress = "127.0.0.1";
      string userHostAddress = "fe80::c813:7781:8925:6297%11";
      List<OptIn.Interface.OptIn> optIns = new List<OptIn.Interface.OptIn>();

      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.BobsBlog, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.BusinessOffers, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.Entertainment, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.MonthlyStatement, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.NonPromotional, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.PostalCommunications, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.RelatedOffers, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.SmsCommunications, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(17, false, string.Empty));

      optIns.ForEach(x => x.Status = true);

      OptInUpdateInfoRequestData request = new OptInUpdateInfoRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, emailAddress, optIns, userHostAddress, firstName, lastName, isReseller);
      OptInUpdateInfoResponseData response = (OptInUpdateInfoResponseData)Engine.Engine.ProcessRequest(request, requestId);

      Assert.IsNotNull(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("App.config")]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("DataProvider.xml")]
    public void OneShopperUpdateAllTurnedOn()
    {
      List<OptInPublicationTypes> requestedOptIns = new List<OptInPublicationTypes>();
      requestedOptIns.Add(OptInPublicationTypes.BobsBlog);
      requestedOptIns.Add(OptInPublicationTypes.BusinessOffers);
      requestedOptIns.Add(OptInPublicationTypes.Entertainment);
      requestedOptIns.Add(OptInPublicationTypes.MonthlyStatement);
      requestedOptIns.Add(OptInPublicationTypes.NonPromotional);
      requestedOptIns.Add(OptInPublicationTypes.PostalCommunications);
      requestedOptIns.Add(OptInPublicationTypes.RelatedOffers);
      requestedOptIns.Add(OptInPublicationTypes.SmsCommunications);

      String emailAddress = "trwalker@godaddy.com";
      String shopperId = SHOPPER_2;
      int requestId = 315;

      bool isReseller = false;
      string lastName = "Walker";
      string firstName = "Tim";
      string userHostAddress = "127.0.0.1";
      List<OptIn.Interface.OptIn> optIns = new List<OptIn.Interface.OptIn>();

      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.BobsBlog, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.BusinessOffers, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.Entertainment, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.MonthlyStatement, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.NonPromotional, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.PostalCommunications, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.RelatedOffers, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.SmsCommunications, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(17, true, string.Empty));

      optIns.ForEach(x => x.Status = false);

      OptInUpdateInfoRequestData request = new OptInUpdateInfoRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, emailAddress, optIns, userHostAddress, firstName, lastName, isReseller);
      OptInUpdateInfoResponseData response = (OptInUpdateInfoResponseData)Engine.Engine.ProcessRequest(request, requestId);

      Assert.IsNotNull(response.IsSuccess);


    }


    [TestMethod]
    [DeploymentItem("App.config")]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("DataProvider.xml")]
    public void TestMethod_SingleItem()
    {

      List<OptInPublicationTypes> requestedOptIns = new List<OptInPublicationTypes>();

      requestedOptIns.Add(OptInPublicationTypes.NonPromotional);

      String emailAddress = "kklink@godaddy.com";
      String shopperId = "858421";
      int requestId = 315;

      bool isReseller = false;
      string lastName = "Klink";
      string firstName = "Kris";
      //string userHostAddress = "127.0.0.1";
      string userHostAddress = "fe80::c813:7781:8925:6297%11";
      List<OptIn.Interface.OptIn> optIns = new List<OptIn.Interface.OptIn>();

      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.NonPromotional, false, string.Empty));

      optIns.ForEach(x => x.Status = true);

      OptInUpdateInfoRequestData request = new OptInUpdateInfoRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, emailAddress, optIns, userHostAddress, firstName, lastName, isReseller);
      OptInUpdateInfoResponseData response = (OptInUpdateInfoResponseData)Engine.Engine.ProcessRequest(request, requestId);

      Assert.IsNotNull(response.IsSuccess);


    }

    [TestMethod]
    [DeploymentItem("App.config")]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("DataProvider.xml")]
    public void TestMethod_SingleDBItem()
    {

      List<OptInPublicationTypes> requestedOptIns = new List<OptInPublicationTypes>();

      requestedOptIns.Add(OptInPublicationTypes.Entertainment);

      String emailAddress = "kklink@godaddy.com";
      String shopperId = "858421";
      int requestId = 315;

      bool isReseller = false;
      string lastName = "Klink";
      string firstName = "Kris";
      string userHostAddress = "127.0.0.1";
      //string userHostAddress = "fe80::c813:7781:8925:6297%11";
      List<OptIn.Interface.OptIn> optIns = new List<OptIn.Interface.OptIn>();

      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.Entertainment, false, string.Empty));

      optIns.ForEach(x => x.Status = true);

      OptInUpdateInfoRequestData request = new OptInUpdateInfoRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, emailAddress, optIns, userHostAddress, firstName, lastName, isReseller);
      OptInUpdateInfoResponseData response = (OptInUpdateInfoResponseData)Engine.Engine.ProcessRequest(request, requestId);

      Assert.IsNotNull(response.IsSuccess);


    }

    [TestMethod]
    [DeploymentItem("App.config")]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("DataProvider.xml")]
    public void TwoShopperUpdate()
    {
      List<OptInPublicationTypes> requestedOptIns = new List<OptInPublicationTypes>();
      requestedOptIns.Add(OptInPublicationTypes.BobsBlog);
      requestedOptIns.Add(OptInPublicationTypes.BusinessOffers);
      requestedOptIns.Add(OptInPublicationTypes.Entertainment);
      requestedOptIns.Add(OptInPublicationTypes.MonthlyStatement);
      requestedOptIns.Add(OptInPublicationTypes.NonPromotional);
      requestedOptIns.Add(OptInPublicationTypes.PostalCommunications);
      requestedOptIns.Add(OptInPublicationTypes.RelatedOffers);
      requestedOptIns.Add(OptInPublicationTypes.SmsCommunications);

      String emailAddress = "trwalker@godaddy.com";
      String shopperId = SHOPPER_1;
      int requestId = 315;

      bool isReseller = false;
      string lastName = "Walker";
      string firstName = "Tim";
      string userHostAddress = "fe80::c813:7781:8925:6297%11";
      List<OptIn.Interface.OptIn> optIns = new List<OptIn.Interface.OptIn>();

      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.BobsBlog, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.BusinessOffers, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.Entertainment, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.MonthlyStatement, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.NonPromotional, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.PostalCommunications, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.RelatedOffers, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.SmsCommunications, false, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(17, false, string.Empty));

      optIns.ForEach(x => x.Status = true);

      OptInUpdateInfoRequestData request1 = new OptInUpdateInfoRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, emailAddress, optIns, userHostAddress, firstName, lastName, isReseller);
      OptInUpdateInfoResponseData response1 = (OptInUpdateInfoResponseData)Engine.Engine.ProcessRequest(request1, requestId);

      Assert.IsNotNull(response1.IsSuccess);


      String shopperId2 = SHOPPER_2;

      List<OptIn.Interface.OptIn> optIns2 = new List<OptIn.Interface.OptIn>();

      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.BobsBlog, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.BusinessOffers, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.Entertainment, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.MonthlyStatement, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.NonPromotional, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.PostalCommunications, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.RelatedOffers, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.SmsCommunications, true, string.Empty));
      optIns.Add(new OptIn.Interface.OptIn(17, false, string.Empty));

      optIns.ForEach(x => x.Status = true);

      OptInUpdateInfoRequestData request2 = new OptInUpdateInfoRequestData(shopperId2, string.Empty, string.Empty, string.Empty, 0, 1, emailAddress, optIns2, userHostAddress, firstName, lastName, isReseller);
      OptInUpdateInfoResponseData response2 = (OptInUpdateInfoResponseData)Engine.Engine.ProcessRequest(request2, requestId);

      Assert.IsNotNull(response2.IsSuccess);
    }


    [TestMethod]
    [DeploymentItem("App.config")]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("DataProvider.xml")]
    public void TestMethod_SingleItem_Phone()
    {
      String emailAddress = "triedy@godaddy.com";
      String shopperId = "859775";
      int requestId = 315;

      bool isReseller = false;
      string lastName = "Riedy";
      string firstName = "Thomas";
      //string userHostAddress = "127.0.0.1";
      string userHostAddress = "fe80::c813:7781:8925:6297%11";
      List<OptIn.Interface.OptIn> optIns = new List<OptIn.Interface.OptIn>();

      optIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.PhoneCommunications, false, string.Empty));

      optIns.ForEach(x => x.Status = true);

      OptInUpdateInfoRequestData request = new OptInUpdateInfoRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, emailAddress, optIns, userHostAddress, firstName, lastName, isReseller);
      OptInUpdateInfoResponseData response = (OptInUpdateInfoResponseData)Engine.Engine.ProcessRequest(request, requestId);

      Assert.IsNotNull(response.IsSuccess);
    }
  }
}
