using Atlantis.Framework.Testing.UnitTesting.BaseClasses;
using System.Collections.Generic;

namespace Atlantis.Framework.Testing.UnitTesting
{
  public interface ITestResults
  {
    List<TestResultBase> TestResults { get; set; }
    TestExtendedLogData ExtendedLogData { get; }
  }
}