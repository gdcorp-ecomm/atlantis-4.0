using Atlantis.Framework.Interface;
using System;
using System.Reflection;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest
{
  public partial class Default : RenderPipelineBasePage
  {
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      AddRenderHandler(new PlaceHolderRenderHandler());

      temp.Text = Assembly.GetAssembly(typeof(System.Web.UI.WebControls.Button)).GetName().FullName;
    }
  }
}