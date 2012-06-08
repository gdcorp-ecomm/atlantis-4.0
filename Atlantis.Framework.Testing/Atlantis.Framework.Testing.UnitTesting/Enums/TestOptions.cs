using System;

namespace Atlantis.Framework.Testing.UnitTesting
{
  [Flags]
  public enum TestOptions
  {
    Standard = 0,
    Destructive = 1,
    Production = 2
  };
}
