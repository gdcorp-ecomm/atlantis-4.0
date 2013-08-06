using System.Web.UI;

namespace WebControls
{
  public class WebControlTwo : Control
  {
    public string Text { get; set; }

    protected override void Render(HtmlTextWriter writer)
    {
      writer.WriteLine("<h1>{0}</h1>", !string.IsNullOrEmpty(Text) ? Text : "ERROR: \"PageLoadText\" parameter not found.");
    }
  }
}