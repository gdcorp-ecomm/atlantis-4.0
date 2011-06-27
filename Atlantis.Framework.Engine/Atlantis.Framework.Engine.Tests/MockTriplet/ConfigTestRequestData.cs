using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Engine.Tests.MockTriplet
{
  public class ConfigTestRequestData : RequestData
  {
    public ConfigTestRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {

    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
