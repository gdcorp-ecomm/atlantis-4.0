using System;

namespace Atlantis.Framework.Testing.UnitTesting.BaseClasses
{
  public class UnitTestLogDataEventArgs : EventArgs
  {
    public string MethodName { get; private set; }
    public string Data { get; private set; }

    public UnitTestLogDataEventArgs(string methodName, string data)
    {
      MethodName = methodName;
      Data = data;
    }
  }
}
