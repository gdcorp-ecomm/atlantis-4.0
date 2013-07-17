using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls
{
  public class WebControlOne : Control
  {
    public string Title { get; set; }

    public string Text { get; set; }

    protected override void Render(HtmlTextWriter writer)
    {
      writer.Write("Web Control One!");
      writer.Write(Title ?? string.Empty);
      writer.Write(Text ?? string.Empty);
    }
  }
}
