using System.Collections.Generic;


namespace Atlantis.Framework.Testing.UnitTesting
{
  public interface ITestResults
  {
    string TestShopperId { get; set; }
    List<TestResultBase> TestResults { get; set; }
    Dictionary<string, string> ExtendedLogData { get; }
  }
}