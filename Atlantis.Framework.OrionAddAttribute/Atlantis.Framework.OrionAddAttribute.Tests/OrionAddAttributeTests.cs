using System;
using System.Collections.Generic;
using Atlantis.Framework.OrionAddAttribute.Interface;
using Atlantis.Framework.OrionAddAttribute.Interface.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.OrionAddAttribute.Tests
{
  [TestClass]
  public class OrionAddAttributeTests
  {
    public OrionAddAttributeTests()
    {
    }

    public TestContext TestContext { get; set; }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestAddAttribute()
    {
      var newAttribute = new OrionAttribute("minute_pack_addon", new List<KeyValuePair<string, string>>
                                                                   {
                                                                     new KeyValuePair<string, string>("num_minutes", "100"),
                                                                     new KeyValuePair<string, string>("expiration_date", DateTime.Today.AddYears(1).ToShortDateString())
                                                                   });
      var request = new OrionAddAttributeRequestData("857527", "sourceUrl", "orderId", "pathway", 1, 1, "5651f1c6-13c9-11e1-9fea-0050569575d8", newAttribute, "MYA");
      var response = Engine.Engine.ProcessRequest(request, 453) as OrionAddAttributeResponseData;

      Assert.IsNotNull(response);
      Assert.IsNull(response.AtlantisException);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(string.IsNullOrWhiteSpace(response.Error));
    }
  }
}
