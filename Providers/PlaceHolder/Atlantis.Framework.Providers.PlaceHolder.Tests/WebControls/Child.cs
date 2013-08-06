using System;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls
{
  public class Child : Control
  {
    private string Text { get; set; }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      Text = "Child Control";
    }

    protected override void Render(HtmlTextWriter writer)
    {
      writer.Write("<div>{0}</div>", Text);
    }
  }
}
