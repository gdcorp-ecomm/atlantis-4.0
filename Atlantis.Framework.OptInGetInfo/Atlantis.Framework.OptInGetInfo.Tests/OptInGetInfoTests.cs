using System;
using System.Diagnostics;
using System.Collections.Generic;
using Atlantis.Framework.OptIn.Interface.Enums;
using Atlantis.Framework.OptInGetInfo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.OptInGetInfo.Tests
{
  [TestClass]
  public class OptInGetInfoTests
  {
    
    [TestMethod]
    [DeploymentItem("App.config")]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("DataProvider.xml")]
    public void TestMethod1()
    {

      List<OptInPublicationTypes> requestedOptIns = new List<OptInPublicationTypes>();
      requestedOptIns.Add(OptInPublicationTypes.BobsBlog );
      requestedOptIns.Add(OptInPublicationTypes.BusinessOffers );
      requestedOptIns.Add(OptInPublicationTypes.Entertainment );
      requestedOptIns.Add(OptInPublicationTypes.MonthlyStatement );
      requestedOptIns.Add(OptInPublicationTypes.NonPromotional );
      requestedOptIns.Add(OptInPublicationTypes.PostalCommunications );
      requestedOptIns.Add(OptInPublicationTypes.RelatedOffers );
      requestedOptIns.Add(OptInPublicationTypes.SmsCommunications );
      requestedOptIns.Add(OptInPublicationTypes.PhoneCommunications);

      String emailAddress = "rhawkinson@godaddy.com";
      String shopperId = "862200";
      OptInGetInfoRequestData request = new OptInGetInfoRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, emailAddress, requestedOptIns);
      OptInGetInfoResponseData response = (OptInGetInfoResponseData) Engine.Engine.ProcessRequest(request, 309);

      Assert.IsNotNull(response.OptIns);
      Assert.IsTrue(response.OptIns.Count > 0);
      Debug.WriteLine(response.ToXML());
    }


    [TestMethod]
    [DeploymentItem("App.config")]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("DataProvider.xml")]
    public void TestMethod_GetSingle()
    {

      List<OptInPublicationTypes> requestedOptIns = new List<OptInPublicationTypes>();
      
      requestedOptIns.Add(OptInPublicationTypes.RelatedOffers);

      String emailAddress = "rhawkinson@godaddy.com";
      String shopperId = "862200";
      OptInGetInfoRequestData request = new OptInGetInfoRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, emailAddress, requestedOptIns);
      OptInGetInfoResponseData response = (OptInGetInfoResponseData)Engine.Engine.ProcessRequest(request, 309);

      Assert.IsNotNull(response.OptIns);
      Assert.IsTrue(response.OptIns.Count > 0);
      Debug.WriteLine(response.ToXML());
    }
  }
}


/*
 862199
 rhawkinson@godaddy.com
 * 
 */