using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.UnitTesting.BaseClasses;
using Atlantis.Framework.Testing.UnitTesting.Enums;

namespace Atlantis.Framework.Testing.UnitTesting.Handlers
{

  public class UnitTestHandler : UnitTestBaseHttpHandler
  {
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

    private string XsltPath
    {
      get { return HttpContext.Current.Request["xsltpath"] ?? string.Empty ; }
    }

    protected virtual bool IsInternal
    {
      get { return SiteContext.IsRequestInternal; }
    }

    private ISiteContext _siteContext;
    protected virtual ISiteContext SiteContext
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

    private IShopperContext _shopperContext;
    protected virtual IShopperContext ShopperContext
    {
      get
      {
        if (_shopperContext == null)
        {
          _shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
        }
        return _shopperContext;
      }
    }


    #region Overrides of BaseHttpHandler

    public override void ProcessRequest(HttpContextBase context)
    {
      string methodName = MethodBase.GetCurrentMethod().Name;
      var result = string.Empty;
      var routeValues = context.Request.RequestContext.RouteData.Values;
      var query = (routeValues["routeQuery"] != null) ? routeValues["routeQuery"].ToString() : string.Empty;


      if (string.IsNullOrWhiteSpace(query))
      {
        ResponseError(404, context);
      }

      try
      {
        if (IsInternal)
        {

          string[] classToTest = query.Split("/".ToCharArray());

          if (classToTest.Length == 0)
          {
            ResponseError(404, context);
          }

          LocalTestRunner = new TestRunner();
          LocalTestRunner.ExecuteTests(string.Concat("UnitTests.", string.Join(".", classToTest)));

          ResponseOutput(context, LocalTestRunner.TestData);
        }
        else
        {
          ResponseError(403, context);
        }

      }
      catch 
      {
        ResponseError(500, context);
      }
    }

    #endregion

    private void ResponseOutput(HttpContextBase context, TestResultData results)
    {
      string json;
      string xmlData = string.Empty;

      var jsonSer = new DataContractJsonSerializer(typeof(TestResultData));
      var xmlSer = new DataContractSerializer(typeof(TestResultData));

      switch (ResponseOutputType)
      {
        case AvailableContentReturnTypes.json:
          try
          {
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
          catch 
          {
            ResponseError(500, context);
          }
          break;

        case AvailableContentReturnTypes.xml:

          try
          {
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
          catch
          {
            ResponseError(500, context);
          }

          break;

        case AvailableContentReturnTypes.xslt:
          try
          {
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
              try
              {
                xslTransform.Load(context.Server.MapPath(xsltPath));
                xslTransform.Transform(doc.CreateNavigator(), null, writer);

                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.StatusCode = 200;
                context.Response.Write(writer.GetStringBuilder().ToString());
              }
              catch
              {
                ResponseError(500, context);
              }
            }
          }
          catch 
          {
            ResponseError(500, context);
          }

          break;

        case AvailableContentReturnTypes.html:


          try
          {
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

              try
              {

                Assembly asm = Assembly.GetExecutingAssembly();
                Stream rsrc = asm.GetManifestResourceStream("Atlantis.Framework.Testing.UnitTesting.Resources.default.xslt");
                using (rsrc)
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
              catch
              {
                ResponseError(500, context);
              }
            }
          }
          catch 
          {
            ResponseError(500, context);
          }
          break;
      }
    }

    private void ResponseError(int statusCode, HttpContextBase context)
    {
      context.Response.Clear();
      context.Response.StatusCode = statusCode;
      context.Response.End();
    }

  }
}
