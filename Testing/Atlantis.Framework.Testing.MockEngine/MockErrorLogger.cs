using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Testing.MockEngine
{
  public class MockErrorLogger : IErrorLogger
  {
    public List<AtlantisException> Exceptions { get; private set; }

    public MockErrorLogger()
    {
      Exceptions = new List<AtlantisException>(10);
    }

    public void LogAtlantisException(AtlantisException atlantisException)
    {
      Exceptions.Add(atlantisException);
    }
  }
}
