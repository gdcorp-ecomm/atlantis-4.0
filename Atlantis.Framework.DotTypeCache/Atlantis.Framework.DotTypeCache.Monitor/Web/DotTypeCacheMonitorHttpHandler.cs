using System;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml.Linq;
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
        ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
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
      string outputType = context.Request.QueryString["responsetype"];
      if (string.IsNullOrEmpty(outputType) || outputType== "xml")
      {
        ResponseXml(context, monitorData);
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
  }
}