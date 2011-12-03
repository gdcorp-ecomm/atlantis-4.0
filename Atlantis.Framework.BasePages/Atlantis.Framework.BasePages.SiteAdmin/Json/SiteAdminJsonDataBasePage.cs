using System;
using System.Text;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Web.UI;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BasePages.SiteAdmin.Json
{
  public abstract class SiteAdminJsonDataBasePage : SiteAdminBasePage
  {
    protected enum RenderModeType
    {
      Undetermined,
      Json,
      DebugPage,
      DebugJson
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
              else if (string.Compare(render, "json", true) == 0)
              {
                _renderMode = RenderModeType.DebugJson;
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

    protected abstract string GetSerializedJson();

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
    }

    protected string SerializeToJson<T>(T dataContractObject)
    {
      string resultJson;
      DataContractJsonSerializer jsSerializer = new DataContractJsonSerializer(typeof(T));
      using (MemoryStream ms = new MemoryStream())
      {
        jsSerializer.WriteObject(ms, dataContractObject);
        resultJson = Encoding.Default.GetString(ms.ToArray());
        ms.Close();
      }

      return resultJson;
    }

    protected void RenderPageContent(HtmlTextWriter writer)
    {
      base.Render(writer);
    }

    protected override void Render(HtmlTextWriter writer)
    {
      if (RenderMode == RenderModeType.DebugPage)
      {
        RenderPageContent(writer);
      }
      else if (RenderMode == RenderModeType.DebugJson)
      {
        string json = GetSerializedJson();
        writer.WriteEncodedText(json);
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
          AtlantisException aex = new AtlantisException("JsonDataBasePage.OnError", Request.Url.ToString(), "0", logMessage, string.Empty, string.Empty, string.Empty, Request.UserHostAddress, SiteContext.Pathway, SiteContext.PageCount);
        }

        WriteSerializedJSON(null, errorMessage);
      }
      catch { }
    }

  }
}
