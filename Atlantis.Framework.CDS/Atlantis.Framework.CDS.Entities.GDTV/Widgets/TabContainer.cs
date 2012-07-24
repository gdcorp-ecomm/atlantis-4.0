using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;
using Atlantis.Framework.CDS.Entities.Common;

namespace Atlantis.Framework.CDS.Entities.GDTV.Widgets
{
  public class TabContainer : IWidgetModel
  {
    //public List<Tab> Tabs { get; set; }

    public TabContainer()
    {
      Tabs = new List<string>();
    }
    public List<string> Tabs { get; set; }

    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }
  }

  public class Tab
  {
    public string Title { get; set; }
    [JsonIgnore]
    public Widget<IWidgetModel> Widget { get; set; }
  }
}
