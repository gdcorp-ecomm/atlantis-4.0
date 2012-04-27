using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.VanityHost.Interface;

namespace Atlantis.Framework.VanityHost.Impl
{
  public class VanityHostRequest : IRequest
  {
    private readonly static XDocument _vanityHostXml;

    static VanityHostRequest()
    {
      _vanityHostXml = null;

      try
      {
        string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string vanityXmlPath = Path.Combine(assemblyFolder, "VanityHost.xml");
        if (File.Exists(vanityXmlPath))
        {
          string vanityXml = File.ReadAllText(vanityXmlPath);
          _vanityHostXml = XDocument.Parse(vanityXml);
        }
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException("VanityHostRequest.LoadXml", "0", ex.Message, ex.StackTrace, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }
    }

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
        if (_vanityHostXml == null)
        {
          result = new VanityHostResponseData(null);
        }
        else
        {
          List<VanityHostItem> items = new List<VanityHostItem>();

          foreach (XElement vanityHostElement in _vanityHostXml.Descendants("VanityHost"))
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
