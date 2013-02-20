using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Filters;

namespace Atlantis.Framework.CDS.Entities.SalesModals
{
  public class FilteredDisclaimerModal : IWidgetModel
  {
    public int ProductGroup { get; set; }

    public List<SimpleFilteredItem> DisclaimerItems { get; set; }
  }
}
