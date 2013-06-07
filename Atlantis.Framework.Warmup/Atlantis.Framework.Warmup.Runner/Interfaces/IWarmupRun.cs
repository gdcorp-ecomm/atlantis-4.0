namespace Atlantis.Framework.Warmup.Runner.Interfaces
{
  using System;
  using System.Collections.Generic;

  using Atlantis.Framework.Warmup.Fixtures;
  using Atlantis.Framework.Warmup.Runner.Results;

  interface IWarmupRun
  {
    IWarmupSetup Setup { set; }

    IList<Type> WarmupClasses { set; }

    WarmupResults Results { get; }

    bool AutoLogResults { set; }
  }
}
