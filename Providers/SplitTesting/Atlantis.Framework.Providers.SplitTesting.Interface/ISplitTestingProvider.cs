using System.Collections.Generic;
using Atlantis.Framework.SplitTesting.Interface;

namespace Atlantis.Framework.Providers.SplitTesting.Interface
{
  public interface ISplitTestingProvider 
  {
    string GetSplitTestingSide(int splitTestId);
    IEnumerable<ActiveSplitTest> GetAllActiveTests { get; }
    string GetTrafficImageData { get; }
    Dictionary<ActiveSplitTest, string> GetTrafficImageDictionary { get; }
  }
}
