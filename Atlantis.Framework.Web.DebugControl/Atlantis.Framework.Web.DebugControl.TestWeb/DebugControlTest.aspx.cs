using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Atlantis.Framework.Web.DebugControl.TestWeb
{
  public partial class DebugControlTest : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Session["debugControlTest"] = "PlaceholderRegex: <head\\s*[\\w\\s\"=:;#-]*>,";
    }
  }

  
}