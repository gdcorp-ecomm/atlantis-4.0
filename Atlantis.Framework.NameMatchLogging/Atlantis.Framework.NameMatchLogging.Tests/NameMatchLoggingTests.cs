using System;
using System.Collections.Generic;
using Atlantis.Framework.NameMatchLogging.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.NameMatchLogging.Tests
{
  [TestClass]
  public class NameMatchLoggingTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void NameMatchLoggingTest()
    {

      List<SuggestedDomain> suggestedDomainList = new List<SuggestedDomain>();
      suggestedDomainList.Add(new SuggestedDomain() { Order = "1", Sld = "stopdaddy", Tld = "com" });
      suggestedDomainList.Add(new SuggestedDomain() { Order = "2", Sld = "fastdaddy", Tld = "com" });
      suggestedDomainList.Add(new SuggestedDomain() { Order = "3", Sld = "slowdaddy", Tld = "com" });

      NameMatchLoggingRequestData requestData = new NameMatchLoggingRequestData("BEE3EE3A-79F7-4158-B50C-0E5D90681B15"
                                                                              , "860430"
                                                                              , "1"
                                                                              , string.Empty
                                                                              , string.Empty
                                                                              , 3
                                                                              , "godaddy.com"
                                                                              , "godaddy"
                                                                              , "com"
                                                                              , "DPP_NAMEMATCH_SPINS"
                                                                              , suggestedDomainList
                                                                              , new TimeSpan(0, 0, 0, 0, 100000));

      NameMatchLoggingResponseData response = (NameMatchLoggingResponseData)Engine.Engine.ProcessRequest(requestData, 390);

      Assert.IsNotNull(response);
      Assert.IsFalse(response.HasError);
    }
  }
}
