using Atlantis.Framework.Interface;
using System;
using System.Threading;

namespace Atlantis.Framework.Engine.Tests.MockTriplet
{
  public class TenPercentFailRequest : IRequest
  {
    static Random _rand;

    static TenPercentFailRequest()
    {
      _rand = new Random();
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      int testValue = _rand.Next(1, 101);
      if (testValue <= 10)
      {
        throw new NotSupportedException("request failure!");
      }

      Thread.Sleep(Math.Max(testValue / 10, 1));

      IResponseData result = new ConfigTestResponseData();
      return result;
    }
  }
}
