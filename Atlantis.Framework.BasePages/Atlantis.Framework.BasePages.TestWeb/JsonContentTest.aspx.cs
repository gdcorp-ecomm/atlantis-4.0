using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Atlantis.Framework.BasePages.Json;

namespace Atlantis.Framework.BasePages.TestWeb
{
  public partial class JsonContentTest : AtlantisContextJsonContentBasePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      string s = string.Empty;
    }

    protected override string GetSerializedJsonData()
    {
      return SerializeToJson("ExtraData");
    }
  }
}
