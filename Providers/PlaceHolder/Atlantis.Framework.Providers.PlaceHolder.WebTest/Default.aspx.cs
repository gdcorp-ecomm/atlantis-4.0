using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest
{
  public partial class Default : RenderPipelineBasePage
  {
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      AddRenderHandler(new PlaceHolderRenderHandler());
    }
  }
}