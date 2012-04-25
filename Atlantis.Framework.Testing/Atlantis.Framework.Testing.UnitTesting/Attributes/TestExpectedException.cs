using System;

namespace Atlantis.Framework.Testing.UnitTesting
{
  public class TestExpectedException : Attribute
  {
    public Type ExceptionType { get; set; }
  }

}
