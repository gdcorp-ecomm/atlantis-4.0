using System.Web.UI;

namespace WebControls
{
  public class WebControlOne : Control
  {
    protected override void Render(HtmlTextWriter writer)
    {
      writer.WriteLine("<h1>Web control one.</h1>");
    }
  }
}