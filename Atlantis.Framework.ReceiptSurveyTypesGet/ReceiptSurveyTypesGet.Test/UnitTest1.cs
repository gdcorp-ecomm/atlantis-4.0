using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ReceiptSurveyTypesGet.Interface;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ReceiptSurveyTypesGet.Impl;
using Atlantis.Framework.DataCache;
using System.Diagnostics;

namespace Atlantis.Framework.ReceiptSurveyTypesGet.Test
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("app.config")]
    public void TestMethod1()
    {
      var response = new ReceiptSurveyTypesGetResponseData(new[] { new SurveyItem("test", "test", "test", 1) });
      var x = response.RandomizeSurveyItems(true);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("app.config")]
    public void GetAll()
    {
      var request = new ReceiptSurveyTypesGetRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "cn");
      var response = DataCache.DataCache.GetProcessRequest(request, 562) as ReceiptSurveyTypesGetResponseData;
      //var response = Engine.Engine.ProcessRequest(request, 562) as ReceiptSurveyTypesGetResponseData;
      Assert.IsTrue(response.IsSuccess);
      var filteredItems = response.RandomizeSurveyItems(true);

      WriteValuesToDebug(filteredItems);
      Debug.WriteLine("NEW");
      
      var request2 = new ReceiptSurveyTypesGetRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "us");
      var response2 = DataCache.DataCache.GetProcessRequest(request2, 562) as ReceiptSurveyTypesGetResponseData;
      //var response = Engine.Engine.ProcessRequest(request, 562) as ReceiptSurveyTypesGetResponseData;
      Assert.IsTrue(response2.IsSuccess);
      var filteredItems2 = response2.RandomizeSurveyItems(true);
      WriteValuesToDebug(filteredItems2);
      string x = "asdf";
      //var p = "pause";

    }

    private void WriteValuesToDebug(List<SurveyItem> items)
    {
      foreach (SurveyItem item in items)
      {
        Debug.WriteLine(item.Text);
      }
    }
  }
}
