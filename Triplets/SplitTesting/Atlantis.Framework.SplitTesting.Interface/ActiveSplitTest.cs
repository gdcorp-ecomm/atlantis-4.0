using System;

namespace Atlantis.Framework.SplitTesting.Interface
{
  public class ActiveSplitTest
  {
    public int TestId { get; set; }
    public int RunId { get; set; }
    public int VersionNumber { get; set; }
    public string EligibilityRules { get; set; }
    public DateTime StartDate { get; set; }
  }
}
