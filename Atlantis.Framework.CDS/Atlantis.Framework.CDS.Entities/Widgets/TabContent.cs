using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Attributes;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  [SectionContainer("Tab Content")]
  public class TabContent : IWidgetModel
  {
    public string TabHeaderText { get; set; }
    public string TabSubHeaderText { get; set; }
    public string TabContainerTemplate { get; set; }
    public int TabIndex { get; set; }
    public bool HasSCAwards { get; set; }
    public int TwoZonesLeftWidth { get; set; }
    public int TwoZonesRightWidth { get; set; }
    public List<Widget<object>> Widgets { get; set; }
    public CurrentTab Tab { get; set; }

    public class CurrentTab
    {
      public string Text { get; set; }
      public string CiCode { get; set; }
    }
  }
}
