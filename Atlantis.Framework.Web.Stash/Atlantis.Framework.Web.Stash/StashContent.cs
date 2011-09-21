using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Atlantis.Framework.Web.Stash
{
  [DefaultProperty("Location")]
  [ToolboxData("<{0}:StashContent runat=server></{0}:StashContent>")]
  public class StashContent : PlaceHolder
  {
    [Category("Behavior")]
    [DefaultValue("")]
    public string Location { get; set; }

    [Category("Behavior")]
    [DefaultValue("")]
    public string RenderKey { get; set; }

    [Category("Behavior")]
    [DefaultValue(false)]
    public bool StashBeforeRender { get; set; }

    public StashContent()
    {
      PreRender += new EventHandler(StashContent_PreRender);
    }

    void StashContent_PreRender(object sender, EventArgs e)
    {
      if (StashBeforeRender)
      {
        StashContext.RenderAndStashContent(this);
      }
    }

    internal void RenderContentsForStash(HtmlTextWriter writer)
    {
      this.RenderChildren(writer);
    }

    protected override void Render(HtmlTextWriter writer)
    {
      if (!StashBeforeRender)
      {
        StashContext.RenderAndStashContent(this);
      }
    }

  }
}
