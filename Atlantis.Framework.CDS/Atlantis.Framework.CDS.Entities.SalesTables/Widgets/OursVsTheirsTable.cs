using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Widgets;

namespace Atlantis.Framework.CDS.Entities.SalesTables.Widgets
{
  public class OursVsTheirsTable : IWidgetModel
  {
    public string Styles { get; set; }
    public int NumberOfColumns { get; set; }
    public List<OursVsTheirsTableColumn> Columns { get; set; }
    public List<OursVsTheirsTableRow> Rows { get; set; }
    public string AboveTableContent { get; set; }
    public string BelowTableContent { get; set; }
    public string JavaScript { get; set; }

    public class OursVsTheirsTableRow : TldElementBase
    {
      public List<string> Row { get; set; }
    }

    public class OursVsTheirsTableColumn : TldElementBase
    {
      public bool IsGoDaddy { get; set; }
      public string Heading { get; set; }
      public string ThAttributes { get; set; }
    }
  }
}
