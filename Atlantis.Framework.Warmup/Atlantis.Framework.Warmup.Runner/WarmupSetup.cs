﻿namespace Atlantis.Framework.Warmup.Runner
{
  using System.Collections.Generic;
  using Atlantis.Framework.Warmup.Fixtures;

  public class WarmupSetup : IWarmupSetup
  {
    private IDictionary<string, object> _Properties;
    public IDictionary<string, object> Properties
    {
      get { return _Properties ?? (_Properties = new Dictionary<string, object>()); }
    }
  }
}
