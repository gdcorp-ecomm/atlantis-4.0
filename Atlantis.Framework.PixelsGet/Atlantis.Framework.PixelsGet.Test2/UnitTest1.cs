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
  [DeploymentItem("pixelsAuGoogle.xml")]
  [DeploymentItem("Atlantis.Framework.PixelsGet.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PixelsGet.Interface.dll")]
  public class UnitTest1
  {
    string _isc = "abc123";
    private int _contextId = 1;
    private string _requestUrl = "http://cart.godaddy.com/order_confirmation";
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
      request.XmlFilePathOverride = System.IO.Directory.GetCurrentDirectory() + "\\PixelsAuGoogle.xml";
      var response = Engine.Engine.ProcessRequest(request, 627) as PixelsGetResponseData;
      Assert.IsTrue(response.Pixels.Count == 1);

      Assert.IsTrue(response.IsSuccess);
    }

    private HttpCookieCollection CreateCookieCollection()
    {
      var collection = new HttpCookieCollection();
      collection.Add(CreateCookie("Cookie1", "Value", 10));
      //  collection.Add(CreateCookie("BlueLithium", "testValue"));
      // collection.Add(CreateCookie("advertisinghp1", string.Empty));
      collection.Add(CreateCookie("GoogleADServices_googleadremarketing", string.Empty, 10));
      return collection;
    }

    private HttpCookieCollection CreateGoogleCookieCollection()
    {
      var collection = new HttpCookieCollection();
      collection.Add(CreateCookie("pixel_googleHP", "lbjjlhodwhegzcaiudraoefahfweccwb", -1));
      //  collection.Add(CreateCookie("BlueLithium", "testValue"));
      // collection.Add(CreateCookie("advertisinghp1", string.Empty));
      collection.Add(CreateCookie("pixel_googledomains", "bdmjtivaifqdafkcgblhejeitfdiifmh", -1));
      return collection;
    }

    private HttpCookie CreateCookie(string name, string value, int minutes)
    {
      var cookie = new HttpCookie(name, value);
      if (minutes == -1)
      {
        cookie.Expires = DateTime.MinValue;
      }
      else
      {
        cookie.Expires = DateTime.Now.AddMinutes(minutes);
      }
      cookie.Path = "/";
      return cookie;
    }
  }
}
