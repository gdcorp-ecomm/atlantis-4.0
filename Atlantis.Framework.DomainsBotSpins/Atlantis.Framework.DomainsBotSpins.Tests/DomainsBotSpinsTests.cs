using System;
using System.Collections.Generic;
using Atlantis.Framework.DomainsBotSpins.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DomainsBotSpins.Tests
{
  [TestClass]
  public class DomainsBotSpinsTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DomainsBotBasicTest()
    {
      List<string> PremiumTLDs = new List<string>();
      PremiumTLDs.Add("com");
      PremiumTLDs.Add("net");

      DomainsBotSpinsRequestData requestData = new DomainsBotSpinsRequestData("godaddy"
                                                                              , PremiumTLDs
                                                                              , 10
                                                                              , null
                                                                              , "prefix=0.25,dash=0.1,suffix=0.25,related=0.5,typo=0.35"
                                                                              , string.Empty
                                                                              , string.Empty
                                                                              , string.Empty
                                                                              , string.Empty
                                                                              , 1);

      DomainsBotSpinsResponseData response = (DomainsBotSpinsResponseData)Engine.Engine.ProcessRequest(requestData, 351);

      string s = response.ToXML();

      Assert.IsNotNull(response);
      Assert.AreNotEqual(0, response.ResultsCount);
            
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DomainsBotAllParametersTest()
    {
      List<string> PremiumTLDs = new List<string>();
      PremiumTLDs.Add("com");
      PremiumTLDs.Add("net");

      DomainsBotSpinsRequestData requestData = new DomainsBotSpinsRequestData("godaddy"
                                                                              , PremiumTLDs
                                                                              , 10
                                                                              , true
                                                                              , string.Empty
                                                                              , "prefix=0.25,dash=0.1,suffix=0.25,related=0.5,typo=0.35"
                                                                              , "EN"
                                                                              , new TimeSpan(0,0,5)
                                                                              , string.Empty
                                                                              , string.Empty
                                                                              , string.Empty
                                                                              , string.Empty
                                                                              , 1);

      DomainsBotSpinsResponseData response = (DomainsBotSpinsResponseData)Engine.Engine.ProcessRequest(requestData, 351);

      Assert.IsNotNull(response);
      Assert.AreEqual(10, response.ResultsCount);
    }
  }
}
