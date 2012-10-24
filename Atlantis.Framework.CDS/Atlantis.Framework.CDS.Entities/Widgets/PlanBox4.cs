using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class PlanBox4 : PlanBox3, IWidgetModel
  {
    public string BelowPlanDetailsCustomContent { get; set; }

    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }
  }
}
