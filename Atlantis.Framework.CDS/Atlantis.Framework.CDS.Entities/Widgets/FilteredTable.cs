using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class FilteredTable : IWidgetModel
  {
    public string Styles { get; set; }
    public string AboveTableContent { get; set; }
    public string TableColsMarkup { get; set; }
    public int NumberOfColumns { get; set; }
    public List<string> TableHeadRow { get; set; }
    public List<FilteredTableRow> Rows { get; set; }
    public bool Filtered { get; set; }
    public string JavaScript { get; set; }
    public int? RegistrationType { get; set; }

    public class FilteredTableRow : TldElementBase
    {
      public List<string> Row { get; set; }
    }
  }
}
