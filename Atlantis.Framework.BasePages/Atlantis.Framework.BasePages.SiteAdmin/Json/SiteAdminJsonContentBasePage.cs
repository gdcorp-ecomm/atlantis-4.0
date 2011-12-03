using System.Text;
using System.IO;
using System.Web.UI;

namespace Atlantis.Framework.BasePages.SiteAdmin.Json
{
  public abstract class SiteAdminJsonContentBasePage : SiteAdminJsonDataBasePage
  {
    private bool _suppressHtml = false;
    protected bool SuppressHtml
    {
      get { return _suppressHtml; }
      set { _suppressHtml = value; }
    }

    public SiteAdminJsonContentBasePage()
    {
      this.PreInit += new System.EventHandler(JsonContentBasePage_PreInit);
    }

    void JsonContentBasePage_PreInit(object sender, System.EventArgs e)
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

    protected abstract string GetSerializedJsonData();

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
      }

      string jsonData = GetSerializedJsonData();
      JsonContent content = new JsonContent(TargetDivId, jsonHtml, jsonData);

      return SerializeToJson(content);
    }
  }
}
