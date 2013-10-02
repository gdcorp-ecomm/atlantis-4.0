using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
// ReSharper disable InconsistentNaming
  public class TldTuiFormGroupLaunchPhase : ITLDTuiFormGroupLaunchPhase
// ReSharper restore InconsistentNaming
  {
    public string Code { get; set; }
    public string PeriodType { get; set; }
  }
}
