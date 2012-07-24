using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;


namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class CheckedList : IWidgetModel
  {
    public CheckedList()
    {
      this.Sections = new List<Section>();
    }

    public List<Section> Sections { get; set; }

    public class Section
    {
      public Section()
      {
        this.ListItems = new List<ListItem>();
      }

      public bool Filtered { get; set; }
      public string Header { get; set; }
      public List<ListItem> ListItems { get; set; }

      public class ListItem : ElementBase { }
    }
  }
}
