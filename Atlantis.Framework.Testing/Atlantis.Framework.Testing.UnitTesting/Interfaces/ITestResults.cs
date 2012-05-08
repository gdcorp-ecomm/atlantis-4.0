using System.Collections.Generic;
using Atlantis.Framework.Testing.UnitTesting.BaseClasses;


namespace Atlantis.Framework.Testing.UnitTesting
{
  public interface ITestResults
  {
    List<TestResultBase> TestResults { get; set; }
    TestExtendedLogData ExtendedLogData { get; }
  }
}