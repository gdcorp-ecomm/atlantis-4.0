namespace Atlantis.Framework.Warmup.Fixtures
{
  using System.Collections.Generic;

  public interface IWarmupSetup
  {
    IDictionary<string, object> Properties { get; }
  }
}
