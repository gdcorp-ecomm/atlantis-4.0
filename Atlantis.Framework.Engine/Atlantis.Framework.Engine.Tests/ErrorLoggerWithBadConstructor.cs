using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Engine.Tests
{
  public class ErrorLoggerWithBadConstructor : IErrorLogger
  {
    public ErrorLoggerWithBadConstructor()
    {
      throw new ApplicationException("BAD");
    }

    public void LogAtlantisException(AtlantisException atlantisException)
    {
      throw new NotImplementedException();
    }
  }
}
