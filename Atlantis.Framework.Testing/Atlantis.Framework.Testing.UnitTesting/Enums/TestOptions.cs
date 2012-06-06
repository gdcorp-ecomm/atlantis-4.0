using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Testing.UnitTesting.Enums
{
  [Flags]
  public enum TestOptions
  {
    Standard = 0,
    Destructive = 1,
    Production = 2
  };
}
