using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class Footer : IWidgetModel
  {
    public Footer()
    {
    }

    private List<Tab> _tabs;
    public List<Tab> Tabs
    {
      get
      {
        if (_tabs == null)
        {
          _tabs = new List<Tab>();
        }
        return _tabs;
      }
    }
    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }

    public class Tab
    {
      public string Name { get; set; }
      public string CiCode { get; set; }
    }
  }
}
