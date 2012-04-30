using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Attributes;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  [SectionContainer("Footer List")]
  public class List : FooterBase, IWidgetModel
  {
    public string Title { get; set; }
    [HideInManagementUI]
    public bool Filtered { get; set; }

    private int _numColumns = 2;
    public int NumColumns
    {
      get { return _numColumns; }
      set { _numColumns = value; }
    }

    public class FooterListItem : ElementBase { }

    public List<FooterListItem> Items { get; set; }
  }
}
