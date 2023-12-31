﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Atlantis.Framework.PixelsGet.Interface;
using Atlantis.Framework.PixelsGet.Interface.Constants;
using Atlantis.Framework.PixelsGet.Interface.PixelObjects;

namespace Atlantis.Framework.PixelsGet.Test.Web
{
  public partial class GetPixels : System.Web.UI.Page
  {
    string _isc = "AD25OFFc";
    private int _contextId = 1;
    private string _requestUrl = "http://www.godaddy.com/fireonthispage.aspx?querstryasdf";
    Dictionary<string, string> _replaceParms = new Dictionary<string, string>();
    private bool _fireOnFirstTimeShopperOnly = true;

    protected void Page_Load(object sender, EventArgs e)
    {
      GetCartSourceCodePixel();
    }

    public void GetCartSourceCodePixel()
    {
      _replaceParms.Add(PixelReplaceTags.PrivateLabelId, "1");
      var request = new PixelsGetRequestData(string.Empty, _requestUrl, string.Empty, string.Empty, 0, "CART",
                                                  _isc, CreateCookieCollection(), _replaceParms, _contextId, _fireOnFirstTimeShopperOnly);
     // request.XmlFilePathOverride = Server.MapPath("~/app_data/pixels.xml");

      XDocument xdoc = XDocument.Load(Server.MapPath("~/app_data/pixels.xml"));

      request.XDocumentOverride = xdoc;

      var response = Engine.Engine.ProcessRequest(request, 627) as PixelsGetResponseData;

      rptPixels.DataSource = response.Pixels;
      rptPixels.DataBind();
    }

    private HttpCookieCollection CreateCookieCollection()
    {
     
      var collection = new HttpCookieCollection();
     collection.Add(CreateCookie("Cookie1", "Value"));
      //  collection.Add(CreateCookie("BlueLithium", "testValue"));
    //  collection.Add(CreateCookie("advertisinghp1", string.Empty));
     //collection.Add(CreateCookie("LP_ValueClick1", string.Empty));
     collection.Add(CreateCookie("LP_ValueClickHP1", string.Empty));
     collection.Add(CreateCookie("GoogleADServices_googleadremarketing", string.Empty));
     //collection.Add(CreateCookie("pixel_googleHP", string.Empty));
     // System.Threading.Thread.Sleep(1000);
     //collection.Add(CreateCookie("pixel_googlewebhosting", string.Empty)); 
     //System.Threading.Thread.Sleep(1000);
     collection.Add(CreateCookie("pixel_googleSSL", string.Empty));
     System.Threading.Thread.Sleep(1000);
     collection.Add(CreateCookie("pixel_googledomains", string.Empty));


      collection.Add(CreateCookie("RadiumOne_domainsearch", string.Empty));
      return collection;
    }

    private HttpCookie CreateCookie(string name, string value)
    {
      var cookie = new HttpCookie(name, value);
      cookie.Expires = DateTime.Now.AddMinutes(10);
      return cookie;
    }

    protected void rptPixels_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
      switch (e.Item.ItemType)
      {
        case ListItemType.Item:
        case ListItemType.AlternatingItem:
          Repeater ciCodeRepeater = (Repeater)e.Item.FindControl("rptCiCodes");
          Pixel pixel = e.Item.DataItem as Pixel;
          ciCodeRepeater.DataSource = pixel.CiCodes;
          ciCodeRepeater.DataBind();
          break;
      }
    }
  }
}