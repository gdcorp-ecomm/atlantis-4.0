using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPPanelContainer : IWidgetModel
  {

    public HPPanelContainer()
    {
      Widgets = new List<Widget<IWidgetModel>>();
    }

    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }
    public string DefaultPanelId { get; set; }

  }
}
