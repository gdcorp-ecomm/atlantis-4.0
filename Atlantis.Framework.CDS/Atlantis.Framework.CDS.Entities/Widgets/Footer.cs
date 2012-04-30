using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;
using Atlantis.Framework.CDS.Entities.Attributes;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  [SectionContainer("Footer")]
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
