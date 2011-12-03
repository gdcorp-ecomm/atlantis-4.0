using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ManagerCategories.Interface;

namespace Atlantis.Framework.ManagerCategories.Tests
{
  [TestClass]
  public class ManagerCategoriesTests
  {
    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.ADODB.dll")]
    public void GetManagerCategories()
    {
      ManagerCategoriesRequestData request = new ManagerCategoriesRequestData(
        string.Empty, string.Empty, string.Empty, string.Empty, 0, 2055);
      ManagerCategoriesResponseData response =
        (ManagerCategoriesResponseData)DataCache.DataCache.GetProcessRequest(request, 462);
    }
  }
}
