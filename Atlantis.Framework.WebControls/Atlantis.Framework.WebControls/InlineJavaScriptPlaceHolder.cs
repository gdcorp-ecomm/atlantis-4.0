using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Atlantis.Framework.WebControls
{
  public class InlineJavaScriptPlaceHolder : PlaceHolder
  {
    private const string HTTP_CONTEXT_KEY = "Atlantis.Framework.WebControls.InlineJavaScriptPlaceHolder.HttpContextKey";

    private string _key = string.Empty;
    /// <summary>
    /// Setting this property will ensure any javascript with the same key will not be rendered more than once
    /// </summary>
    public string Key
    {
      get { return _key; }
      set { _key = value; }
    }

    private string RenderContents()
    {
      StringBuilder htmStringBuilder = new StringBuilder();
      HtmlTextWriter htmlTextWriter = new HtmlTextWriter(new StringWriter(htmStringBuilder, CultureInfo.InvariantCulture));

      foreach (Control childControl in Controls)
      {
        childControl.RenderControl(htmlTextWriter);
      }

      return htmStringBuilder.ToString();
    }

    protected override void Render(HtmlTextWriter writer)
    {
      IDictionary<string, string> controlDictionary = HttpContext.Current.Items[HTTP_CONTEXT_KEY] as Dictionary<string, string>;
      if (controlDictionary == null)
      {
        controlDictionary = new Dictionary<string, string>(128);
        controlDictionary.Add(Key, RenderContents());
      }
      else
      {
        if (controlDictionary.ContainsKey(Key))
        {
          // If the Key is not set, then this javascript will always be appended
          // If the Key was set, then this javascript will not be appended more than once.
          if (Key == string.Empty)
          {
            controlDictionary[Key] += RenderContents();
          }
        }
        else
        {
          controlDictionary.Add(Key, RenderContents());
        }
      }

      HttpContext.Current.Items[HTTP_CONTEXT_KEY] = controlDictionary;

      writer.Close();
      writer.Dispose();
    }
  }
}
