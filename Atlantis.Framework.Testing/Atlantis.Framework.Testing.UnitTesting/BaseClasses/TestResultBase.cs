
namespace Atlantis.Framework.Testing.UnitTesting
{
  public class TestResultBase
  {
    public TestResultBase(bool? success, string testName, string result)
    {
      Success = success;
      TestName = testName;
      Result = result;
    }

    public bool? Success { get; set; }
    public string TestName { get; set; }
    public string Result { get; set; }
  }
}
