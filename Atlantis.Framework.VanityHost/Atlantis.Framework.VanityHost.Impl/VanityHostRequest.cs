using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.VanityHost.Impl.Data;
using Atlantis.Framework.VanityHost.Interface;

namespace Atlantis.Framework.VanityHost.Impl
{
  public class VanityHostRequest : IRequest
  {
    // <VanityHost domain="supportwebsite" dottype="com" linkcontext="1" linkname="SITEURL" redirect="gdshop/support.asp" redirecttype="302" />
    //   <VanityHost domain="godaddy" dottype="biz" linkcontext="1" linkname="SITEURL" redirect="tlds/biz.aspx">
    //    <queryitem name="tld" value="biz" />
    //    <queryitem name="isc" value="diggbiz" />
    //  </VanityHost>

    private string GetValue(XElement element, string attributeName)
    {
      string result = null;
      XAttribute attribute = element.Attribute(attributeName);
      if (attribute != null)
      {
        result = attribute.Value;
      }
      return result;
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        string vanityHostXml = VanityHostData.VanityHostXml;
        if (string.IsNullOrEmpty(vanityHostXml))
        {
          result = new VanityHostResponseData(null);
        }
        else
        {
          List<VanityHostItem> items = new List<VanityHostItem>();

          XDocument vanityHostDoc = XDocument.Parse(vanityHostXml);
          foreach (XElement vanityHostElement in vanityHostDoc.Descendants("VanityHost"))
          {
            string domain = GetValue(vanityHostElement, "domain");
            string dottype = GetValue(vanityHostElement, "dottype");
            string linkcontext = GetValue(vanityHostElement, "linkcontext");
            string linkname = GetValue(vanityHostElement, "linkname");
            string redirect = GetValue(vanityHostElement, "redirect");
            string redirecttype = GetValue(vanityHostElement, "redirecttype");

            int linkContextId;
            if (!int.TryParse(linkcontext, out linkContextId))
            {
              continue;
            }

            bool isPermanentRedirect = !"302".Equals(redirecttype);

            NameValueCollection queryItems = new NameValueCollection();
            foreach (XElement queryElement in vanityHostElement.Descendants("queryitem"))
            {
              string name = GetValue(queryElement, "name");
              string value = GetValue(queryElement, "value");
              if ((name != null) && (value != null))
              {
                queryItems[name] = value;
              }
            }

            VanityHostItem item = new VanityHostItem(domain, dottype, linkname, redirect, isPermanentRedirect, linkContextId, queryItems);
            items.Add(item);
          }

          result = new VanityHostResponseData(items);
        }
      }
      catch (Exception ex)
      {
        result = new VanityHostResponseData(requestData, ex);
      }

      return result;
    }

  }
}
