using System;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using Atlantis.Framework.BasePages;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public abstract class ModalBase : UserControl
  {
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

        protected enum RenderModeType
    {
      Undetermined,
      Json,
      DebugPage
    }

    private RenderModeType _renderMode = RenderModeType.Undetermined;

    protected RenderModeType RenderMode
    {
      get
      {
        if (_renderMode == RenderModeType.Undetermined)
        {
          _renderMode = RenderModeType.Json;
          if (SiteContext.IsRequestInternal)
          {
            if (Request["render"] != null)
            {
              string render = Request["render"];
              if (string.Compare(render, "page", true) == 0)
              {
                _renderMode = RenderModeType.DebugPage;
              }
            }
          }
        }

        return _renderMode;
      }
      set
      {
        _renderMode = value;
      }
    }

    protected virtual string CallBack
    {
      get
      {
        string result = string.Empty;
        if (Request["callback"] != null)
        {
          result = Request["callback"];
        }

        return result;
      }
    }

    private int _maxJsonStringLength = 2097152;
    protected int MaxJsonStringLength
    {
      get
      {
        return _maxJsonStringLength;
      }
      set
      {
        _maxJsonStringLength = value;
      }
    }

    protected string SerializeToJson<T>(T dataContractObject)
    {
      string resultJson = string.Empty;
      JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
      jsonSerializer.MaxJsonLength = MaxJsonStringLength;
      resultJson = jsonSerializer.Serialize(dataContractObject);
      return resultJson;
    }

    protected void RenderPageContent(HtmlTextWriter writer)
    {
      base.Render(writer);
    }

    public ModalBase()
    {
      this.Load += new EventHandler(AtlantisContextJsonContentBaseControl_Load);
    }

    void AtlantisContextJsonContentBaseControl_Load(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(TargetDivId) && SiteContext.IsRequestInternal)
      {
        RenderMode = RenderModeType.DebugPage;
      }
    }

    public virtual string TargetDivId
    {
      get
      {
        string result = string.Empty;
        if (Request["targetDivId"] != null)
        {
          result = Request["targetDivId"];
        }

        return result;
      }
    }

    protected override void Render(HtmlTextWriter writer)
    {
      if (RenderMode == RenderModeType.DebugPage)
      {
        RenderPageContent(writer);
      }
      else
      {
        WriteSerializedJSON(writer, string.Empty);
      }
    }

    protected virtual void WriteSerializedJSON(HtmlTextWriter writer, string errorMessage)
    {
      Response.ContentType = "application/json";
      string json = string.Empty;

      if (string.IsNullOrEmpty(errorMessage))
      {
        json = GetSerializedJson();
      }
      else
      {
        StringBuilder sb = new StringBuilder();
        sb.Append("{\"Error\":\"");
        sb.Append(Server.HtmlEncode(errorMessage.Replace('\n', ' ').Replace('\r', ' ')));
        sb.Append("\"}");
        json = sb.ToString();
      }

      if (CallBack.Length > 0)
      {
        Response.ContentType = "text/javascript";
        json = string.Concat(CallBack, "(", json, ")");
      }

      if (writer != null)
      {
        writer.Write(json);
      }
      else
      {
        Response.Write(json);
      }
    }

    private string GetSerializedJsonHtmlContent()
    {
      string jsonHtml = string.Empty;

      StringBuilder sb = new StringBuilder();
      using (TextWriter tw = new StringWriter(sb))
      {
        using (HtmlTextWriter formWriter = new HtmlTextWriter(tw, string.Empty))
        {
          formWriter.Indent = 0;
          RenderPageContent(formWriter);
          tw.Flush();
        }
      }
      jsonHtml = sb.ToString();
      return jsonHtml;
    }    

    protected string GetSerializedJson()
    {
      string jsonHtml = GetSerializedJsonHtmlContent();

      CDSJsonContent content = new CDSJsonContent(TargetDivId, jsonHtml);

      return SerializeToJson(content);
    }

    protected override void OnError(EventArgs e)
    {
      string errorMessage = "Error";
      Exception ex = Server.GetLastError();
      Server.ClearError();

      try
      {
        if (ex != null)
        {
          Exception baseEx = ex.GetBaseException();
          if (baseEx != null)
          {
            ex = baseEx;
          }

          if (SiteContext.IsRequestInternal)
          {
            errorMessage = ex.ToString();
          }

          string logMessage = ex.Message + Environment.NewLine + ex.StackTrace;
          AtlantisException aex = new AtlantisException("AtlantisContextJsonContentBaseControl.OnError", Request.Url.ToString(), "0", logMessage, string.Empty, ShopperContext.ShopperId, string.Empty, Request.UserHostAddress, SiteContext.Pathway, SiteContext.PageCount);
          Engine.Engine.LogAtlantisException(aex);
        }

        WriteSerializedJSON(null, errorMessage);
      }
      catch { }
    }
  }
}
