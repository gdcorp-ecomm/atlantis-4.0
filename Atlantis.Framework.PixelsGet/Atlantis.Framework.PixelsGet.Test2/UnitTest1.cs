using System;
using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.PixelsGet.Interface;
using Atlantis.Framework.PixelsGet.Interface.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PixelsGet.Test
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("pixels.xml")]
  public class UnitTest1
  {
    string _isc = "abc123";
    private int _contextId = 1;
    private string _requestUrl = "http://www.godaddy.com/fireonthispage.aspx";
    Dictionary<string, string> _replaceParms = new Dictionary<string, string>();

    [TestMethod]
    public void GetCartSourceCodePixel()
    {
      _replaceParms.Add(Atlantis.Framework.PixelsGet.Interface.Constants.PixelReplaceTags.PrivateLabelId, "1");
      var request = new PixelsGetRequestData(string.Empty, _requestUrl, string.Empty, string.Empty, 0, "CART",
                                                  _isc, CreateCookieCollection(), _replaceParms, _contextId);
      var response = Engine.Engine.ProcessRequest(request, 627) as PixelsGetResponseData;

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void GetGoogleDuplicateSourceCodePixel()
    {
      _replaceParms.Add(Atlantis.Framework.PixelsGet.Interface.Constants.PixelReplaceTags.PrivateLabelId, "1");
      var request = new PixelsGetRequestData(string.Empty, _requestUrl, string.Empty, string.Empty, 0, "CART",
                                                  _isc, CreateGoogleCookieCollection(), _replaceParms, _contextId);
      var response = Engine.Engine.ProcessRequest(request, 627) as PixelsGetResponseData;

      Assert.IsTrue(response.IsSuccess);
    }

    private HttpCookieCollection CreateCookieCollection()
    {
      var collection = new HttpCookieCollection();
      collection.Add(CreateCookie("Cookie1", "Value"));
      //  collection.Add(CreateCookie("BlueLithium", "testValue"));
      // collection.Add(CreateCookie("advertisinghp1", string.Empty));
      collection.Add(CreateCookie("GoogleADServices_googleadremarketing", string.Empty));
      return collection;
    }

    private HttpCookieCollection CreateGoogleCookieCollection()
    {
      var collection = new HttpCookieCollection();
      collection.Add(CreateCookie("Cookie1", "Value"));
      //  collection.Add(CreateCookie("BlueLithium", "testValue"));
      // collection.Add(CreateCookie("advertisinghp1", string.Empty));
      collection.Add(CreateCookie("GoogleADServices_googleadremarketing", string.Empty));
      return collection;
    }

    private HttpCookie CreateCookie(string name, string value)
    {
      var cookie = new HttpCookie(name, value);
      cookie.Expires = DateTime.Now.AddMinutes(10);
      cookie.Path = "/";
      return cookie;
    }
  }
}
