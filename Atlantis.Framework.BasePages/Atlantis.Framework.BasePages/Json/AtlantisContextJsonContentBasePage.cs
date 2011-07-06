using System;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Atlantis.Framework.BasePages.Json
{
  public abstract class AtlantisContextJsonContentBasePage : AtlantisContextJsonDataBasePage
  {
    private bool _suppressHtml = false;
    protected bool SuppressHtml
    {
      get { return _suppressHtml; }
      set { _suppressHtml = value; }
    }

    public AtlantisContextJsonContentBasePage()
    {
      this.PreInit += new EventHandler(AtlantisContextJsonContentBasePage_PreInit);
    }

    void AtlantisContextJsonContentBasePage_PreInit(object sender, EventArgs e)
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

    [Obsolete("Implement GetJsonDataObject - this is deprecated")]
    protected virtual string GetSerializedJsonData()
    {
      return null;
    }

    private JsonContentPlaceholder FindJsonContentPlaceholder()
    {
      JsonContentPlaceholder result = null;
      if (Context.Items.Contains(JsonContentPlaceholder.JsonContentPlaceHolderContextKey))
      {
        result = Context.Items[JsonContentPlaceholder.JsonContentPlaceHolderContextKey] as JsonContentPlaceholder;
      }
      return result;
    }

    private string GetSerializedJsonHtmlContent(JsonContentPlaceholder placeHolder)
    {
      StringBuilder sb = new StringBuilder();
      using (TextWriter tw = new StringWriter(sb))
      {
        using (HtmlTextWriter formWriter = new HtmlTextWriter(tw, string.Empty))
        {
          formWriter.Indent = 0;
          placeHolder.RenderControl(formWriter);
          tw.Flush();
        }
      }

      return sb.ToString();
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

      Regex insertEx = new Regex("<!--JSONCONTENT-->(?<InsertContent>.*?)<!--JSONCONTENT-->", RegexOptions.Singleline | RegexOptions.Compiled);
      Match match = insertEx.Match(sb.ToString());

      if (match.Success)
      {
        jsonHtml = match.Groups[1].Captures[0].Value;
      }

      return jsonHtml;
    }

    protected virtual JsonSerializationType JsonSerializationTypeUsed
    {
      get
      {
        return JsonSerializationType.JavaScript;
      }
    }    

    protected sealed override string GetSerializedJson()
    {
      string jsonHtml = string.Empty;
      if (!SuppressHtml)
      {
        JsonContentPlaceholder placeHolder = FindJsonContentPlaceholder();
        if (placeHolder != null)
        {
          jsonHtml = GetSerializedJsonHtmlContent(placeHolder);
        }
        else
        {
          jsonHtml = GetSerializedJsonHtmlContent();
        }
      }

      object _jsonObject = GetJsonDataObject();
      if (_jsonObject == null)
      {
        string jsonData = GetSerializedJsonData();
        if (!string.IsNullOrEmpty(jsonData))
        {
          JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
          jsonSerializer.MaxJsonLength = MaxJsonStringLength;
          _jsonObject = jsonSerializer.DeserializeObject(jsonData);
        }
      }
      JsonContent content = new JsonContent(TargetDivId, jsonHtml, _jsonObject);

      return SerializeToJson(content, JsonSerializationTypeUsed);
    }

    public virtual object GetJsonDataObject()
    {
      return null;
    }
  }
}
