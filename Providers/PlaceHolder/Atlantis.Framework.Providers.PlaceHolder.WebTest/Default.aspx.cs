using System;
using Atlantis.Framework.Web.RenderPipeline;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest
{
  public partial class Default : RenderPipelineBasePage
  {
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      AddRenderHandlers(new PlaceHolderRenderHandler());
    }
  }
}