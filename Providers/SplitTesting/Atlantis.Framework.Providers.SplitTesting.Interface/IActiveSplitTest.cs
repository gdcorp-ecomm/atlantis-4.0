using System;

namespace Atlantis.Framework.Providers.SplitTesting.Interface
{
  public interface IActiveSplitTest
  {
    int TestId { get; set; }
    int RunId { get; set; }
    int VersionNumber { get; set; }
    string EligibilityRules { get; set; }
    DateTime StartDate { get; set; }
  }
}
