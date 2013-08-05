using System.Collections.Generic;

namespace Atlantis.Framework.Providers.SplitTesting.Interface
{
  public interface ISplitTestingProvider 
  {
    IActiveSplitTestSide GetSplitTestingSide(int splitTestId);
    IEnumerable<IActiveSplitTest> GetAllActiveTests { get; }
    string GetTrackingData { get; }
    Dictionary<IActiveSplitTest, IActiveSplitTestSide> GetTrackingDictionary { get; }
    bool SetOverrideSide(int splitTestId, string overrideSideName);
  }
}
