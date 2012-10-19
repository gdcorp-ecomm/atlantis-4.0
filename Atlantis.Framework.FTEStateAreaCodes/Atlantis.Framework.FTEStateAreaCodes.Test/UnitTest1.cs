using System;
using Atlantis.Framework.FTEStateAreaCodes.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.FTEStateAreaCodes.Test
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("app.config")]
    public void TestMethod1()
    {
      var request = new FTEStateAreaCodesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "US-CO");
      var response = DataCache.DataCache.GetProcessRequest(request, 612) as FTEStateAreaCodesResponseData;

      Console.WriteLine(response.ToString());
    }
  }
}
