using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Engine.Tests
{
  public class EngineTestsLogger : IErrorLogger
  {
    List<AtlantisException> _exceptions = new List<AtlantisException>();

    public EngineTestsLogger()
    {
    }

    public void LogAtlantisException(AtlantisException atlantisException)
    {
      if (atlantisException != null)
      {
        _exceptions.Add(atlantisException);
      }
    }

    public IEnumerable<AtlantisException> ExceptionsLogged
    {
      get { return _exceptions; }
    }

    public int ExceptionCount
    {
      get { return _exceptions.Count; }
    }

    public void ClearExceptions()
    {
      _exceptions.Clear();
    }
  }
}
