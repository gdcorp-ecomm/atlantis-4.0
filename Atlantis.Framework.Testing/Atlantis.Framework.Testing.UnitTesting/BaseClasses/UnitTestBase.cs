using Atlantis.Framework.Testing.UnitTesting.BaseClasses;

namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for UnitTestPageBase
  /// </summary>
  public class UnitTestBase
  {

    public delegate void LogData(object sender, UnitTestLogDataEventArgs e);
    public event LogData Log;

    private void OnLog(object sender, UnitTestLogDataEventArgs e)
    {
      if (Log != null)
      {
        Log(sender,e);
      }
    }

    public void AddLogData(string methodName, string data)
    {
      var e = new UnitTestLogDataEventArgs(methodName,data);

      OnLog(methodName,e);
    }
  }
}
