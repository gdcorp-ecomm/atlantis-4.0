using System;
using System.Collections.Generic;
using Atlantis.Framework.NameMatch.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.NameMatch.Tests
{
  [TestClass]
  public class NameMatchTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void NameMatchBasicTest()
    {
      List<string> PremiumTLDs = new List<string>();
      PremiumTLDs.Add("com");
      PremiumTLDs.Add("net");

      NameMatchRequestData requestData = new NameMatchRequestData("thorntoncarsracingjohncars"
                                                                  , PremiumTLDs
                                                                  , 10
                                                                  , null
                                                                  , "prefix=0.25,dash=0.1,suffix=0.25,related=0.5,typo=0.35"
                                                                  , "103113"
                                                                  , string.Empty
                                                                  , string.Empty
                                                                  , string.Empty
                                                                  , 1
                                                                  , true
                                                                  , true
                                                                  , "un,pre"
                                                                  , "s,ing"
                                                                  , "1"
                                                                  , true
                                                                  , "127.0.0.1"
                                                                  , "1"
                                                                  , "dpp_namematch"
                                                                  , "1.0");
      requestData.RequestTimeout = new TimeSpan(0, 0, 0, 0, 100000);
      NameMatchResponseData response = (NameMatchResponseData)Engine.Engine.ProcessRequest(requestData, 388);

      string s = response.ToXML();

      Assert.IsNotNull(response);
      Assert.AreNotEqual(0, response.ResultsCount);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void NameMatchAllParametersTest()
    {
      List<string> PremiumTLDs = new List<string>();
      PremiumTLDs.Add("com");
      PremiumTLDs.Add("net");

      NameMatchRequestData requestData = new NameMatchRequestData("godaddy"
                                                                  , PremiumTLDs
                                                                  , 10
                                                                  , true
                                                                  , string.Empty
                                                                  , "prefix=0.25,dash=0.1,suffix=0.25,related=0.5,typo=0.35"
                                                                  , "EN"
                                                                  , new TimeSpan(0, 0, 5)
                                                                  , string.Empty
                                                                  , string.Empty
                                                                  , string.Empty
                                                                  , string.Empty
                                                                  , 1
                                                                  , true
                                                                  , true
                                                                  , "un,pre"
                                                                  , "s,ing"
                                                                  , "auth"
                                                                  , true
                                                                  , "127.0.0.1"
                                                                  , "1"
                                                                  , "dpp_namematch"
                                                                  , "0.9");

      NameMatchResponseData response = (NameMatchResponseData)Engine.Engine.ProcessRequest(requestData, 388);

      Assert.IsNotNull(response);
      Assert.AreEqual(10, response.ResultsCount);
    }
  }
}
