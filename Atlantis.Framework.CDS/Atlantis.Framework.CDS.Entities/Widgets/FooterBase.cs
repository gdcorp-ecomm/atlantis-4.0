using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Filters;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class FooterBase : FilterBase
  {
    public string TabText { get; set; }
    public string CiCode { get; set; }
  }
}
