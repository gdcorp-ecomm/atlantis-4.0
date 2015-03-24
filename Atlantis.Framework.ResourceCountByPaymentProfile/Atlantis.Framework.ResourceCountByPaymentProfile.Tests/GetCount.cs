using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ResourceCountByPaymentProfile.Interface;

namespace Atlantis.Framework.ResourceCountByPaymentProfile.Tests
{
  [TestClass]
  public class GetCount
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ResourceCountByPaymentProfile.Impl.dll")]
    public void GetAllProfileOneShopperCountTest()
    {
      var profileIdList = new List<int>
      {
        58628, 59597, 60651, 60844, 61061, 61459, 61460, 61523, 61541, 61546, 61829, 61866, 61879, 61880, 61886, 64215, 66245
      };

      foreach (var profileId in profileIdList)
      {
        var start = DateTime.Now;
        var request = new ResourceCountByPaymentProfileRequestData("856907", string.Empty, string.Empty, string.Empty, 0, profileId);
        var response = (ResourceCountByPaymentProfileResponseData)Engine.Engine.ProcessRequest(request, 650);
        var finish = DateTime.Now;
        var elapsed = (finish - start).TotalMilliseconds;

        Debug.Write(string.Format("Total Records: {0} | Elapsed Time in Milliseconds: {1}\n", response.TotalRecords, elapsed));
        Assert.IsTrue(response.IsSuccess);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ResourceCountByPaymentProfile.Impl.dll")]
    public void GetOneProfileManyShopperCountTest()
    {
      var shopperProfileIdList = new List<Tuple<string, int>>
      {
        new Tuple<string, int>("850774", 61795),
        new Tuple<string, int>("857517", 59937),
        new Tuple<string, int>("856907", 59597),
        new Tuple<string, int>("842749", 58079),
        new Tuple<string, int>("857623", 60847),
        new Tuple<string, int>("862666", 60563),
        new Tuple<string, int>("858346", 60379),
        new Tuple<string, int>("861894", 60556),
        new Tuple<string, int>("861896", 60378),
        new Tuple<string, int>("854138", 59569),
        new Tuple<string, int>("863139", 60699),
        new Tuple<string, int>("840748", 59816)
      };

      foreach (var tuplePair in shopperProfileIdList)
      {
        var start = DateTime.Now;
        var request = new ResourceCountByPaymentProfileRequestData(tuplePair.Item1, string.Empty, string.Empty, string.Empty, 0, tuplePair.Item2);
        var response = (ResourceCountByPaymentProfileResponseData)Engine.Engine.ProcessRequest(request, 650);
        var finish = DateTime.Now;
        var elapsed = (finish - start).TotalMilliseconds;

        Debug.Write(string.Format("Shopper: {0} | Profile: {1} | Total Records: {2} | Elapsed Time in Milliseconds: {3}\n", tuplePair.Item1, tuplePair.Item2, response.TotalRecords, elapsed));
        Assert.IsTrue(response.IsSuccess);
      }
    }
  }
}
