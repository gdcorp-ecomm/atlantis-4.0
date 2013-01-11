using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Filters;

namespace Atlantis.Framework.CDS.Entities.Content.Widgets
{
  public class FilteredMagicBox : IWidgetModel
  {
    public List<SimpleFilteredItem> ContentBlocks { get; set; }
  }
}
