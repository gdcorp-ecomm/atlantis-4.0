using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Atlantis.Framework.TH.Domains.Tests
{
  [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
  public class MockDebugProvider : ProviderBase, IDebugContext
  {
    public MockDebugProvider(IProviderContainer container)
      : base(container)
    {
    }

    public List<KeyValuePair<string, string>> GetDebugTrackingData()
    {
      throw new NotImplementedException();
    }

    public string GetQaSpoofQueryValue(string spoofParamName)
    {
      throw new NotImplementedException();
    }

    public void LogDebugTrackingData(string key, string data)
    {
      Console.WriteLine("{0}:{1}", key, data);
    }
  }
}
