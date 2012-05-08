using System;

namespace Atlantis.Framework.Testing.UnitTesting.Exceptions
{
  [Serializable]
  class InvalidTestFixtureException : Exception
  {
    
    public InvalidTestFixtureException() : base()
    {
    }

    public InvalidTestFixtureException(string message) : base(message)
    {
    }

    public InvalidTestFixtureException(string message, Exception innerException) : base(message, innerException)
    {
    }

  }
}
