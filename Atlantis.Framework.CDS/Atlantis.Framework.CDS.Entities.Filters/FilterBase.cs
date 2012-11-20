using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Entities.Filters
{
    public class FilterBase
    {
      public string Filters { get; set; }
    }

    public class SimpleFilteredItem : FilterBase
    {
      public string Text { get; set; }
    }
}
