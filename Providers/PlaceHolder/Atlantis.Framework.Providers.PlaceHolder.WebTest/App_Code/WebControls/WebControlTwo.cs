using System.Web.UI;

namespace WebControls
{
  public class WebControlTwo : Control
  {
    protected override void Render(HtmlTextWriter writer)
    {
      writer.WriteLine("<h1>Web control two.</h1>");
    }
  }
}