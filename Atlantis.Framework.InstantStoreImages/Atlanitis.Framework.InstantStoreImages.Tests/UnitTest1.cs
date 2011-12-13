using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.InstantStoreImages.Interface;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Nimitz;

namespace Atlanitis.Framework.InstantStoreImages.Tests
{
  public enum ConnectTypeEnum
  {
    CONNECT_TYPE_NET = 0,
    CONNECT_TYPE_ODBC = 1,
    CONNECT_TYPE_OLE = 2,
    CONNECT_TYPE_WEB_SERVICE = 3,
    CONNECT_TYPE_JDBC = 4,
    CONNECT_TYPE_XML = 5,
    CONNECT_TYPE_DELIMITED = 6,
  }
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestBackgroundImages()
    {
      string result = string.Empty;

      try
      {
        result = NetConnect.LookupConnectInfo("InstantPage.API", "simplesite.test.client.godaddy.com", "InstantPage", "test", ConnectLookupType.WebService);
      }
      catch (System.Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.ToString());
      }
      InstantStoreImagesRequestData requestData = new InstantStoreImagesRequestData(string.Empty, string.Empty, string.Empty, string.Empty,0);
      InstantStoreImageResponseData responseData = (InstantStoreImageResponseData)Engine.ProcessRequest(requestData, 292);
      System.Diagnostics.Debug.WriteLine(responseData.BackgroundImages[0].Categories);
    }
  }
}
