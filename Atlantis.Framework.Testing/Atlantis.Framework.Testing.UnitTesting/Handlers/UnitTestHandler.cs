using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.UnitTesting.BaseClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

namespace Atlantis.Framework.Testing.UnitTesting.Handlers
{

  public sealed class UnitTestHandler : UnitTestBaseHttpHandler
  {
    private static bool _allowExternalRequestsToRunTests = false;
    /// <summary>
    /// If set to true ALL unit tests will be runable from external requests.  This is NOT a setting for single tests.
    /// Do NOT set this to true on public facing websites. It is an override for internal sites only that 
    /// cannot set ISiteContext.IsRequestInternal = true.
    /// </summary>
    public static bool AllowExternalRequestsToRunTests
    {
      get { return _allowExternalRequestsToRunTests; }
      set { _allowExternalRequestsToRunTests = value; }
    }

    private static string _unitTestNamespace;
    public static string UnitTestNamespace
    {
      get
      {
        return _unitTestNamespace ?? (_unitTestNamespace = "UnitTests");
      }
      set
      {
        _unitTestNamespace = value;
      }
    }

    private static string[] _unitTestAssemblies;
    public static string[] UnitTestAssemblies
    {
      get
      {
        return _unitTestAssemblies ?? (_unitTestAssemblies = new string[] { "App_Code" });
      }
      set
      {
        _unitTestAssemblies = value;
      }
    }

    private TestRunner LocalTestRunner { get; set; }
    private AvailableContentReturnTypes ResponseOutputType
    {
      get
      {
        AvailableContentReturnTypes temp;
        if (!Enum.TryParse<AvailableContentReturnTypes>(HttpContext.Current.Request["responsetype"], true, out temp))
        {
          temp = AvailableContentReturnTypes.xml;
        }
        return temp;
      }
    }

    private HashSet<string> TestMethods
    {
      get
      {
        var testMethods = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var methods = HttpContext.Current.Request["testmethod"];
        if (!string.IsNullOrEmpty(methods))
        {
          var separators = new[] { ",", "|" };
          foreach (var method in methods.Split(separators, StringSplitOptions.RemoveEmptyEntries))
          {
            testMethods.Add(method.Trim());
          }
        }
        return testMethods;
      }
    }

    private string XsltPath
    {
      get { return HttpContext.Current.Request["xsltpath"] ?? string.Empty; }
    }

    private ISiteContext _siteContext;
    private ISiteContext SiteContext
    {
      get
      {
        if (_siteContext == null)
        {
          _siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
        }
        return _siteContext;
      }
    }


    #region Overrides of BaseHttpHandler

    public override void ProcessRequest(HttpContextBase context)
    {
      string methodName = MethodBase.GetCurrentMethod().Name;
      var routeValues = context.Request.RequestContext.RouteData.Values;
      var query = (routeValues["routeQuery"] != null) ? routeValues["routeQuery"].ToString() : string.Empty;


      if (string.IsNullOrWhiteSpace(query))
      {
        ResponseError(404, context);
      }

      try
      {
        bool allowedToProceed = (AllowExternalRequestsToRunTests || SiteContext.IsRequestInternal);
        if (allowedToProceed)
        {

          string[] classToTest = query.Split("/".ToCharArray());

          if (classToTest.Length == 0)
          {
            ResponseError(404, context);
          }

          LocalTestRunner = new TestRunner();
          LocalTestRunner.UnitTestAssemblies = UnitTestAssemblies;

          LocalTestRunner.ExecuteTests(UnitTestNamespace, string.Join(".", classToTest), TestMethods);

          ResponseOutput(context, LocalTestRunner.TestData);
        }
        else
        {
          ResponseError(403, context);
        }

      }
      catch (Exception ex)
      {
        ResponseError(500, context, ex);
      }
    }

    #endregion

    private void ResponseJson(HttpContextBase context, TestResultData results)
    {
      string json = string.Empty;
      var jsonSer = new DataContractJsonSerializer(typeof(TestResultData));

      using (var ms = new MemoryStream())
      {
        jsonSer.WriteObject(ms, results);
        json = String.Format("[{0}]", Encoding.Default.GetString(ms.ToArray()));
        ms.Close();
      }

      context.Response.ContentType = "application/json";
      context.Response.ContentEncoding = Encoding.UTF8;
      context.Response.StatusCode = 200;
      context.Response.Write(json);
    }

    private void ResponseXml(HttpContextBase context, TestResultData results)
    {
      string xmlData = string.Empty;
      var xmlSer = new DataContractSerializer(typeof(TestResultData));

      using (var ms1 = new MemoryStream())
      {
        xmlSer.WriteObject(ms1, results);
        xmlData = Encoding.Default.GetString(ms1.ToArray());
        ms1.Close();
      }

      context.Response.ContentType = "text/xml";
      context.Response.ContentEncoding = Encoding.UTF8;
      context.Response.StatusCode = 200;
      context.Response.Write(xmlData);
    }

    private void ResponseXslt(HttpContextBase context, TestResultData results)
    {
      string xmlData = string.Empty;
      var xmlSer = new DataContractSerializer(typeof(TestResultData));

      using (var ms2 = new MemoryStream())
      {
        xmlSer.WriteObject(ms2, results);
        xmlData = Encoding.Default.GetString(ms2.ToArray());
        ms2.Close();
      }

      var xsltPath = VirtualPathUtility.ToAbsolute(XsltPath);

      XslCompiledTransform xslTransform = new XslCompiledTransform();
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlData);

      using (var writer = new StringWriter())
      {
        xslTransform.Load(context.Server.MapPath(xsltPath));
        xslTransform.Transform(doc.CreateNavigator(), null, writer);

        context.Response.ContentEncoding = Encoding.UTF8;
        context.Response.StatusCode = 200;
        context.Response.Write(writer.GetStringBuilder().ToString());
      }
    }

    private void ResponseHtml(HttpContextBase context, TestResultData results)
    {
      string xmlData = string.Empty;
      var xmlSer = new DataContractSerializer(typeof(TestResultData));

      using (var ms3 = new MemoryStream())
      {
        xmlSer.WriteObject(ms3, results);
        xmlData = Encoding.Default.GetString(ms3.ToArray());
        ms3.Close();
      }

      using (var writer1 = new StringWriter())
      {
        XslCompiledTransform xslTransform1 = new XslCompiledTransform();
        XmlDocument doc1 = new XmlDocument();

        doc1.LoadXml(xmlData);

        Assembly asm = Assembly.GetExecutingAssembly();
        using (Stream rsrc = asm.GetManifestResourceStream("Atlantis.Framework.Testing.UnitTesting.Resources.default.xslt"))
        {
          using (var xmlReader = new XmlTextReader(rsrc))
          {
            xslTransform1.Load(xmlReader);
            xslTransform1.Transform(doc1.CreateNavigator(), null, writer1);
          }
        }

        context.Response.ContentType = "text/html";
        context.Response.ContentEncoding = Encoding.UTF8;
        context.Response.StatusCode = 200;
        context.Response.Write(writer1.GetStringBuilder().ToString());
      }
    }

    private void ResponseOutput(HttpContextBase context, TestResultData results)
    {
      switch (ResponseOutputType)
      {
        case AvailableContentReturnTypes.json:
          ResponseJson(context, results);
          break;

        case AvailableContentReturnTypes.xml:
          ResponseXml(context, results);
          break;

        case AvailableContentReturnTypes.xslt:
          ResponseXslt(context, results);
          break;

        case AvailableContentReturnTypes.html:
          ResponseHtml(context, results);
          break;
      }
    }

    private void ResponseError(int statusCode, HttpContextBase context)
    {
      ResponseError(statusCode, context, null);
    }

    private void ResponseError(int statusCode, HttpContextBase context, Exception ex)
    {
      bool isErrorValid = true;
      string message = null;

      if (ex != null)
      {
        if (ex.GetType() == typeof(ThreadAbortException))
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

  }
}
