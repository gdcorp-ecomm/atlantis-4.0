using System;
using Atlantis.Framework.GadgetsVote.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GadgetsVote.Tests
{
  [TestClass]
  public class GadgetsVoteTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GadgetsVoteForHinch()
    {
      const string shopperId = "858421";
      const int requestType = 529;
      TimeSpan requestTimeout = TimeSpan.FromSeconds(20);
      const int voteHinch = 1;

      RequestData requestData = new GadgetsVoteRequestData(shopperId,
                                                           "http://localhost",
                                                           Int32.MinValue.ToString(),
                                                           "localhost",
                                                           1,
                                                           voteHinch,
                                                           requestTimeout);

      try
      {
        var getGadgetsVoteResponseData =
          (GadgetsVoteResponseData) Engine.Engine.ProcessRequest(requestData, requestType);

        Console.WriteLine(getGadgetsVoteResponseData.ToXML());
        Assert.IsTrue(!getGadgetsVoteResponseData.HasError);
        Assert.IsTrue(getGadgetsVoteResponseData.VotingResults.Count == 2);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GadgetsVoteForDanica()
    {
      const string shopperId = "858421";
      const int requestType = 529;
      TimeSpan requestTimeout = TimeSpan.FromSeconds(20);
      const int voteDanica = 2;

      RequestData requestData = new GadgetsVoteRequestData(shopperId,
                                                           "http://localhost",
                                                           Int32.MinValue.ToString(),
                                                           "localhost",
                                                           1,
                                                           voteDanica,
                                                           requestTimeout);

      try
      {
        var getGadgetsVoteResponseData =
          (GadgetsVoteResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

        Console.WriteLine(getGadgetsVoteResponseData.ToXML());
        Assert.IsTrue(!getGadgetsVoteResponseData.HasError);
        Assert.IsTrue(getGadgetsVoteResponseData.VotingResults.Count == 2);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GadgetsVoteForInvalid()
    {
      const string shopperId = "858421";
      const int requestType = 529;
      TimeSpan requestTimeout = TimeSpan.FromSeconds(20);
      const int voteError = -1;

      RequestData requestData = new GadgetsVoteRequestData(shopperId,
                                                           "http://localhost",
                                                           Int32.MinValue.ToString(),
                                                           "localhost",
                                                           1,
                                                           voteError,
                                                           requestTimeout);

      try
      {
        var getGadgetsVoteResponseData =
          (GadgetsVoteResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

        Console.WriteLine(getGadgetsVoteResponseData.ToXML());
        Assert.IsTrue(getGadgetsVoteResponseData.HasError);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
