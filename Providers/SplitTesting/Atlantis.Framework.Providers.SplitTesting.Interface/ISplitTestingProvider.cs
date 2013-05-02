using System.Collections.Generic;
using Atlantis.Framework.SplitTesting.Interface;

namespace Atlantis.Framework.Providers.SplitTesting.Interface
{
  public interface ISplitTestingProvider
  {
    string GetSplitTestingSide(int splitTestId);
    Dictionary<ActiveSplitTest, string> GetActiveTestsForTrackingData { get; }
    IEnumerable<ActiveSplitTest> GetAllActiveTests { get; }
  }
}
