using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Atlantis.Framework.Providers.RenderPipeline.Tests.WebControl
{
  class ThreadAbort : Control
  {
    protected override void OnLoad(EventArgs e)
    {
      Thread.CurrentThread.Abort();
    }
  }
}
