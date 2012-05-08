using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Testing.UnitTesting.BaseClasses
{
  [DataContract(IsReference = false, Name = "TestResultData", Namespace = "")]
  public class TestResultData : ITestResults
  {
    private TestExtendedLogData _extLogData;
    private List<TestResultBase> _testResults;

    [DataMember]
    public List<TestResultBase> TestResults
    {
      get
      {
        if (_testResults == null)
        {
          _testResults = new List<TestResultBase>();
        }
        return _testResults;
      }
      set { _testResults = value; }
    }

    [DataMember]
    public TestExtendedLogData ExtendedLogData
    {
      get
      {
        if (_extLogData == null)
        {
          _extLogData = new TestExtendedLogData();
        }
        return _extLogData;
      }
    }
  }
}
