using System;
using Atlantis.Framework.Providers.SplitTesting.Interface;

namespace Atlantis.Framework.SplitTesting.Interface
{
  public class ActiveSplitTest : IActiveSplitTest
  {
    public int TestId { get; set; }
    public int RunId { get; set; }
    public int VersionNumber { get; set; }
    public string EligibilityRules { get; set; }
    public DateTime StartDate { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
      {
        return false;
      }

      var activeSplitTest = obj as ActiveSplitTest;
      if (activeSplitTest == null)
      {
        return false;
      }

      return (RunId == activeSplitTest.RunId);
    }

    public override int GetHashCode()
    {
      return RunId.GetHashCode();
    }
  }
}
