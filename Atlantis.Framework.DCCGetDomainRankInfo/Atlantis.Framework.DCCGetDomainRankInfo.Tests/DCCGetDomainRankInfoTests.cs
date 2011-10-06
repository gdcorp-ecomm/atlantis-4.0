using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Atlantis.Framework.DCCGetDomainRankInfo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DCCGetDomainRankInfo.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetDCCGetDomainRankInfoTests
  {
    private const string _shopperId = "83439";  //Dev: 856907  Test: 83439
    private const int _requestType = 238;
    private const int _asyncRequestType = 239;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCGetDomainRankInfoSuccessTest()
    {
      Dictionary<int, string> domains = new Dictionary<int, string>();
      //DEV
      //domains.Add(1665622, "intrepidkjs.com");
      //domains.Add(3, "blah.com");
      //TEST
      domains.Add(1624874, "freeunlimitedhosting.info");
      domains.Add(1624876, "freeunlimitedhosting.net");
      domains.Add(3, "blah.com");

      DCCGetDomainRankInfoRequestData request = new DCCGetDomainRankInfoRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "MYA"
         , true
         , domains);

      DCCGetDomainRankInfoResponseData response = (DCCGetDomainRankInfoResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      foreach (DomainRankInfo dri in response.DomainRankInfos.Values)
      {
        Debug.WriteLine(string.Format("Date Scored: {0}", dri.DateScored.ToString()));
        Debug.WriteLine(string.Format("Domain Diagnostic Url: {0}", dri.DomainDiagnosticUrl));
        Debug.WriteLine(string.Format("Domain Name: {0}", dri.DomainName));
        Debug.WriteLine(string.Format("Processing Status: {0}", dri.ProcessingStatus));
        Debug.WriteLine(string.Format("Rank: {0}", dri.Rank));
        Debug.WriteLine(string.Format("Shopper Id: {0}", dri.ShopperId));
        Debug.WriteLine(Environment.NewLine);
        Debug.WriteLine("****************************************");
      }

      Debug.WriteLine(Environment.NewLine);
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    private volatile bool _asyncSearchComplete = false;
    private DCCGetDomainRankInfoResponseData _asyncResponse = null;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCGetDomainRankInfoAsyncTest()
    {
      _asyncResponse = null;

      Dictionary<int, string> domains = new Dictionary<int, string>();
      //DEV
      //domains.Add(1665622, "intrepidkjs.com");
      //domains.Add(3, "blah.com");
      //TEST
      domains.Add(1624874, "freeunlimitedhosting.info");
      domains.Add(1624876, "freeunlimitedhosting.net");
      domains.Add(3, "blah.com");

      DCCGetDomainRankInfoRequestData request = new DCCGetDomainRankInfoRequestData(_shopperId
       , string.Empty
       , string.Empty
       , string.Empty
       , 0
       , "MYA"
       , true
       , domains);

      IAsyncResult asyncResult = Engine.Engine.BeginProcessRequest(request, _asyncRequestType, EndDCCGetDomainRankInfoAsyncTest, null);
      while (!_asyncSearchComplete)
      {
        Thread.Sleep(TimeSpan.FromMilliseconds(500));
      }

      foreach (DomainRankInfo dri in _asyncResponse.DomainRankInfos.Values)
      {
        Debug.WriteLine(string.Format("Date Scored: {0}", dri.DateScored.ToString()));
        Debug.WriteLine(string.Format("Domain Diagnostic Url: {0}", dri.DomainDiagnosticUrl));
        Debug.WriteLine(string.Format("Domain Name: {0}", dri.DomainName));
        Debug.WriteLine(string.Format("Processing Status: {0}", dri.ProcessingStatus));
        Debug.WriteLine(string.Format("Rank: {0}", dri.Rank));
        Debug.WriteLine(string.Format("Shopper Id: {0}", dri.ShopperId));
        Debug.WriteLine(Environment.NewLine);
        Debug.WriteLine("****************************************");
      }

      Debug.WriteLine(Environment.NewLine);
      Debug.WriteLine(_asyncResponse.ToXML());

      Assert.IsNotNull(_asyncResponse);
      Assert.IsTrue(_asyncResponse.IsSuccess);
    }

    private void EndDCCGetDomainRankInfoAsyncTest(IAsyncResult result)
    {
      _asyncResponse = (DCCGetDomainRankInfoResponseData)Engine.Engine.EndProcessRequest(result);
      _asyncSearchComplete = true;
    }

    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //public void DCCGetDomainRankInfoFailTest()
    //{
    //  SessionCache.ContextWrapper.UseMockContext = true;

    //  Dictionary<int, bool> domains = new Dictionary<int, bool>();

    //  DCCGetDomainRankInfoRequestData request = new DCCGetDomainRankInfoRequestData(_shopperId
    //     , string.Empty
    //     , string.Empty
    //     , string.Empty
    //     , 0
    //     , "MYA"
    //     , domains);

    //  DCCGetDomainRankInfoResponseData response = SessionCache.SessionCache.GetProcessRequest<DCCGetDomainRankInfoResponseData>(request, _requestType);

    //  Debug.WriteLine(response.GetException());
    //  Assert.IsFalse(response.IsSuccess);
    //}
  }
}
