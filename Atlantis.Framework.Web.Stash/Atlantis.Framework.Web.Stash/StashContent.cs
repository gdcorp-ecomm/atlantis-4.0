using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Atlantis.Framework.Web.Stash
{
  public class StashContent : PlaceHolder
  {
    public string Location { get; set; }
    public string RenderKey { get; set; }
    public bool StashBeforeRender { get; set; }

    public StashContent()
    {
      Init += new EventHandler(StashContent_Init);
    }

    void StashContent_Init(object sender, EventArgs e)
    {
      Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);
    }

    void Page_PreRenderComplete(object sender, EventArgs e)
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
