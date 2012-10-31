using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class LogoBarContainer : IWidgetModel
  {
    private List<Widget<IWidgetModel>> _widgets;
    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets
    {
      get { return _widgets ?? (_widgets = new List<Widget<IWidgetModel>>()); }
      set { _widgets = value; }
    }
  }
}

