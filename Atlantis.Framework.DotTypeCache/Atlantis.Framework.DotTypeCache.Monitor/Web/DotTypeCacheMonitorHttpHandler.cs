using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.DotTypeCache.Monitor.Web
{
  public class DotTypeCacheMonitorHttpHandler : IHttpHandler
  {
    public void ProcessRequest(HttpContext context)
    {
      try
      {
        if ((IsRequestInternal))
        {
          string methodName = MethodBase.GetCurrentMethod().Name;
          var routeValues = context.Request.RequestContext.RouteData.Values;
          var query = (routeValues["routeQuery"] != null) ? routeValues["routeQuery"].ToString() : string.Empty;

          if (string.IsNullOrEmpty(query))
          {
            ResponseError(404, context);
          }

          MonitorDataTypes dataType;
          if (!Enum.TryParse(query, true, out dataType))
          {
            ResponseError(404, context);
          }

          XDocument monitorData = MonitorData.GetMonitorData(dataType, context.Request.QueryString);
          if (monitorData != null)
          {
            ResponseOutput(context, monitorData, dataType);
          }
          else
          {
            ResponseError(500, context);
          }
        }
        else
        {
          ResponseError(403, context);
        }

      }
      catch (ThreadAbortException)
      { }
      catch (Exception ex)
      {
        ResponseError(500, context, ex);
      }
    }

    public bool IsReusable { get { return false; } }

    private bool IsRequestInternal
    {
      get
      {
        var siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
        return siteContext.IsRequestInternal;
      }
    }

    private void ResponseError(int statusCode, HttpContext context)
    {
      ResponseError(statusCode, context, null);
    }

    private void ResponseError(int statusCode, HttpContext context, Exception ex)
    {
      bool isErrorValid = true;
      string message = null;

      if (ex != null)
      {
        if (ex is ThreadAbortException)
        {
          isErrorValid = false;
        }
        else
        {
          message = ex.Message + Environment.NewLine + ex.StackTrace;
        }
      }

      if (isErrorValid)
      {
        context.Response.Clear();
        context.Response.StatusCode = statusCode;

        if (!string.IsNullOrEmpty(message))
        {
          context.Response.Write(message);
        }

        context.Response.End();
      }
    }

    private void ResponseOutput(HttpContext context, XDocument monitorData, MonitorDataTypes dataType)
    {
      var outputType = context.Request.QueryString["responsetype"];
      if (string.IsNullOrEmpty(outputType) || outputType.ToLowerInvariant() == "xml" || dataType.ToString().ToLowerInvariant() != "help")
      {
        ResponseXml(context, monitorData);
      }
      else
      {
        ResponseHtml(context, monitorData, dataType);
      }
    }

    private void ResponseXml(HttpContext context, XDocument monitorData)
    {
      string xmlData = monitorData.ToString(SaveOptions.None);

      context.Response.ContentType = "text/xml";
      context.Response.ContentEncoding = Encoding.UTF8;
      context.Response.StatusCode = 200;
      context.Response.Write(xmlData);
    }

    private void ResponseHtml(HttpContext context, XDocument monitorData, MonitorDataTypes dataType)
    {
      var transform = new XslCompiledTransform();

      Assembly asm = Assembly.GetExecutingAssembly();
      string resourcePath = "Atlantis.Framework.DotTypeCache.Monitor.Resources." + dataType.ToString().ToLowerInvariant() + ".xslt";
      using (Stream resource = asm.GetManifestResourceStream(resourcePath))
      {
        if (resource != null)
        {
          using (var xmlReader = new XmlTextReader(resource))
          {
            transform.Load(xmlReader);
          }
        }
      }

      string output;

      using (var stringWriter = new StringWriter())
      {
        using (var outputWriter = new XmlHtmlWriter(stringWriter))
        {
          transform.Transform(monitorData.CreateReader(), outputWriter);
        }
        output = stringWriter.GetStringBuilder().ToString();
      }

      context.Response.ContentType = "text/html";
      context.Response.ContentEncoding = Encoding.UTF8;
      context.Response.StatusCode = 200;
      context.Response.Write(output);
    }

    private class XmlHtmlWriter : XmlTextWriter
    {
      string _openingElement = string.Empty;
      readonly HashSet<string> _fullyClosedElements = new HashSet<string>(new string[] { "br", "hr" });

      public XmlHtmlWriter(TextWriter writer)
        : base(writer)
      {
      }

      public override void WriteEndElement()
      {
        if (!_fullyClosedElements.Contains(_openingElement))
          WriteFullEndElement();
        else
          base.WriteEndElement();
      }

      public override void WriteStartElement(string prefix, string localName, string ns)
      {
        base.WriteStartElement(prefix, localName, ns);
        _openingElement = localName;
      }
    } 
  }
}