using System;
using Atlantis.Framework.BasePages.Json;

namespace Atlantis.Framework.BasePages.TestWeb
{
  public partial class JsonDataTest : AtlantisContextJsonDataBasePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected override string GetSerializedJson()
    {
      return SerializeToJson("Hello world");
    }
  }
}
