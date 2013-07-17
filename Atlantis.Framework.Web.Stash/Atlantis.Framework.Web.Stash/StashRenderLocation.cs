using System.Web.UI.WebControls;
using System.Web.UI;

namespace Atlantis.Framework.Web.Stash
{
  public class StashRenderLocation : PlaceHolder
  {
    public string Location { get; set; }

    protected override void Render(HtmlTextWriter writer)
    {
      if (!string.IsNullOrEmpty(Location))
      {
        string html = StashContext.GetRenderedStashContent(Location);
        if (!string.IsNullOrEmpty(html))
        {
          writer.WriteLine(html);
        }
      }
    }
  }
}