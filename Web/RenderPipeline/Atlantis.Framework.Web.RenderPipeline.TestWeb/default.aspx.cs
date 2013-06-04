using Atlantis.Framework.Web.RenderPipeline.TestWeb.TestRenderHandlers;
using System;

namespace Atlantis.Framework.Web.RenderPipeline.TestWeb
{
  public partial class _default : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      litA.Text = "This is a literal control that is {{green}}";
      PreRender += _default_PreRender;
    }

    void _default_PreRender(object sender, EventArgs e)
    {
      pipeline1.AddRenderHandlers(new SquigglyRenderHandler());
    }

    protected string Green
    {
      get { return "{{green}}"; }
    }
  }
}