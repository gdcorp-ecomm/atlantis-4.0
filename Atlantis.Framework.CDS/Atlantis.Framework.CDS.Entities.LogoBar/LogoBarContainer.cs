using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class LogoBarContainer : IWidgetModel
  {
    public string SpriteUri { get; set; }

    private IList<Widget<IWidgetModel>> _widgets;
    [JsonIgnore]
    public IList<Widget<IWidgetModel>> Widgets
    {
      get { return _widgets ?? (_widgets = new List<Widget<IWidgetModel>>()); }
      set { _widgets = value; }
    }

    public string Width { get; set; }
  }
}

