using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public enum TabContainerTemplates
  {
    OneMainZone = 0,
    TwoZonesSideBySide = 1
  }

  public class TabContent : IWidgetModel
  {
    public TabContent()
    {
    }

    public string TabHeaderText { get; set; }
    public string TabSubHeaderText { get; set; }
    public string TabContainerTemplate { get; set; }
    public int TabIndex { get; set; }
    public bool HasSCAwards { get; set; }
    public int TwoZonesLeftWidth { get; set; }
    public int TwoZonesRightWidth { get; set; }
    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }

    private CurrentTab _tab;
    public CurrentTab Tab
    {
      get 
      {
        if (_tab == null)
        {
          _tab = new CurrentTab();
        }
        return _tab; 
      }
      set { _tab = value; }
    }

    public class CurrentTab
    {
      public string Text { get; set; }
      public string CiCode { get; set; }
    }
  }
}
