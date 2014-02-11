using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DataCache.Tests.Mocks
{
  public class MockCachedTripletRequestData : RequestData
  {
    public string RequestValue { get; set; }

    public override string GetCacheMD5()
    {
      return String.IsNullOrEmpty(RequestValue) ? "NO_REQUEST_VALUE" : RequestValue;
    }
  }
}
