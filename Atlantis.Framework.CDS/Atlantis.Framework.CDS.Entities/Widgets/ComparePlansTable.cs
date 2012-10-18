using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class ComparePlansTable : IWidgetModel
  {
    public List<ComparePlansTableColumn> Columns { get; set; }
    public List<ComparePlansTableRow> Rows { get; set; }
    public string JavaScript { get; set; }

    public class ComparePlansTableRow : TldElementBase
    {
      public bool IsBold { get; set; }
      public bool IsSubheading { get; set; }
      public List<string> Row { get; set; }
    }

    public class ComparePlansTableColumn : TldElementBase
    {
      public int Pfid { get; set; }
      public bool AddToCartButton { get; set; }
      public string Heading { get; set; }
      public string Styles { get; set; }
    }
  }
}
