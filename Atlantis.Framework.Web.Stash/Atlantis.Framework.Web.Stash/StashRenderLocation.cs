using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;

namespace Atlantis.Framework.Web.Stash
{
  [DefaultProperty("Location")]
  [ToolboxData("<{0}:StashRenderLocation runat=server />")]
  public class StashRenderLocation : PlaceHolder
  {
    [Category("Behavior")]
    [DefaultValue("")]
    public string Location { get; set; }

    protected override void Render(HtmlTextWriter writer)
    {
      string html = StashContext.GetRenderedStashContent(Location);
      if (!string.IsNullOrEmpty(html))
      {
        writer.WriteLine(html);
      }
    }
  }
}
