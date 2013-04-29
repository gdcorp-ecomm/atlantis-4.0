using System;
using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.PixelsGet.Interface;
using Atlantis.Framework.PixelsGet.Interface.PixelObjects;
using Atlantis.Framework.PixelsGet.Interface.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PixelsGet.Test
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("pixels.xml")]
  [DeploymentItem("Receipt.xml")]
  [DeploymentItem("pixelsAuGoogle.xml")]
  [DeploymentItem("PixelsMultipleCI.xml")]
  [DeploymentItem("pixelsSponsorPay.xml")]
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

    private HttpCookieCollection CreateGoogleCookieCollection()
    {
      var collection = new HttpCookieCollection();
      collection.Add(CreateCookie("pixel_googleHP", "lbjjlhodwhegzcaiudraoefahfweccwb", -1));
      //  collection.Add(CreateCookie("BlueLithium", "testValue"));
      // collection.Add(CreateCookie("advertisinghp1", string.Empty));
      collection.Add(CreateCookie("pixel_googledomains", "bdmjtivaifqdafkcgblhejeitfdiifmh", -1));
      return collection;
    }

    [TestMethod]
    [DeploymentItem("ReceiptItemTrigger.xml")]
    [DeploymentItem("PixelItemTrigger.xml")]
    public void GetItemTrigger()
    {
      _isc = "USAD2013";
      var collection = new HttpCookieCollection();
      collection.Add(CreateCookie("pixel_US_RocketFuel1", "gjpgreiibeqhfaqaoefhyhkjwemdvipj", -1));
      _replaceParms.Add(Atlantis.Framework.PixelsGet.Interface.Constants.PixelReplaceTags.PrivateLabelId, "1");
      var request = new PixelsGetRequestData(string.Empty, _requestUrl, string.Empty, string.Empty, 0, "CART",
                                                  _isc, collection, _replaceParms, _contextId);
      request.FirstTimeShopper = false;
      request.XmlFilePathOverride = System.IO.Directory.GetCurrentDirectory() + "\\PixelItemTrigger.xml";
      request.OrderXml = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "\\ReceiptItemTrigger.xml");
      var response = Engine.Engine.ProcessRequest(request, 627) as PixelsGetResponseData;
      Assert.IsTrue(response.IsSuccess);
      foreach (Pixel pixel in response.Pixels)
      {
        Assert.IsTrue(pixel.Name.Contains("Rocket"));
      }
    }

    [TestMethod]
    [DeploymentItem("ReceiptNoItemTrigger.xml")]
    [DeploymentItem("PixelItemTrigger.xml")]
    public void GetNoItemTrigger()
    {
      _isc = "USAD2013";
      var collection = new HttpCookieCollection();
      collection.Add(CreateCookie("pixel_US_RocketFuel1", "gjpgreiibeqhfaqaoefhyhkjwemdvipj", -1));
      _replaceParms.Add(Atlantis.Framework.PixelsGet.Interface.Constants.PixelReplaceTags.PrivateLabelId, "1");
      var request = new PixelsGetRequestData(string.Empty, _requestUrl, string.Empty, string.Empty, 0, "CART",
                                                  _isc, collection, _replaceParms, _contextId);
      request.FirstTimeShopper = false;
      request.XmlFilePathOverride = System.IO.Directory.GetCurrentDirectory() + "\\PixelItemTrigger.xml";
      request.OrderXml = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "\\ReceiptNoItemTrigger.xml");
      var response = Engine.Engine.ProcessRequest(request, 627) as PixelsGetResponseData;
      Assert.IsTrue(response.Pixels.Count == 0);
    }

    [TestMethod]
    public void GetTrialPayNotFire()
    {
      _isc = "spp2";
      _replaceParms.Add(Atlantis.Framework.PixelsGet.Interface.Constants.PixelReplaceTags.PrivateLabelId, "1");
      var request = new PixelsGetRequestData(string.Empty, _requestUrl, string.Empty, string.Empty, 0, "CART",
                                                  _isc, CreateSponsorPayCookieCollection(), _replaceParms, _contextId);
      request.FirstTimeShopper = true;
      request.XmlFilePathOverride = System.IO.Directory.GetCurrentDirectory() + "\\Pixels.xml";
      request.OrderXml = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "\\Receipt.xml");
      var response = Engine.Engine.ProcessRequest(request, 627) as PixelsGetResponseData;
      Assert.IsTrue(response.IsSuccess);
      foreach (Pixel pixel in response.Pixels)
      {
        Assert.IsTrue(!pixel.Name.Contains("Trial"));
      }
    }

    private HttpCookieCollection CreateSponsorPayCookieCollection()
    {
      var collection = new HttpCookieCollection();
      collection.Add(CreateCookie("pixeldata1", "SPSBBE=", 10));
      return collection;
    }

    [TestMethod]
    public void GetMultipleCI()
    {
      _replaceParms.Add(Atlantis.Framework.PixelsGet.Interface.Constants.PixelReplaceTags.PrivateLabelId, "1");
      var request = new PixelsGetRequestData(string.Empty, _requestUrl, string.Empty, string.Empty, 0, "CART",
                                                  _isc, CreateBlueLithiumCookieCollection(), _replaceParms, _contextId);
      request.FirstTimeShopper = true;
      request.XmlFilePathOverride = System.IO.Directory.GetCurrentDirectory() + "\\PixelsMultipleCI.xml";
      request.OrderXml = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "\\Receipt.xml");
      var response = Engine.Engine.ProcessRequest(request, 627) as PixelsGetResponseData;
      Assert.IsTrue(response.IsSuccess);
      foreach (Pixel pixel in response.Pixels)
      {
        Assert.IsTrue(pixel.CiCodes.Count == 3);
      }
    }

    private HttpCookieCollection CreateBlueLithiumCookieCollection()
    {
      var collection = new HttpCookieCollection();
      collection.Add(CreateCookie("pixeldata1", "BlueLithium=", 10));
      return collection;
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
