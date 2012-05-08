
using System.Runtime.Serialization;

namespace Atlantis.Framework.Testing.UnitTesting
{
  [DataContract(IsReference=false,Name="TestResult", Namespace="")]
  public class TestResultBase
  {
    public TestResultBase()
    {
    }

    public TestResultBase(bool? success, string testName, string result)
    {
      Success = success;
      TestName = testName;
      Result = result;
    }

    [DataMember]
    public bool? Success { get; set; }

    [DataMember]
    public string TestName { get; set; }

    [DataMember]
    public string Result { get; set; }
  }
}
