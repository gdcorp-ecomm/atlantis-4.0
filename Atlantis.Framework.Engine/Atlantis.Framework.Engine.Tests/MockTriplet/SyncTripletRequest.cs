using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Engine.Tests.MockTriplet
{
  public class SyncTripletRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string returnValue = ((TestTripletRequestData)requestData).ResultValue;

      if ("beginerror" == returnValue)
      {
        throw new ApplicationException("beginerror");
      }

      if ("beginerroratlantis" == returnValue)
      {
        throw new AtlantisException("UnitTests", 0, "beginerroratlantis", string.Empty);
      }

      if ("null" == returnValue)
      {
        return null;
      }

      return new TestTripletResponseData(returnValue);
    }
  }
}
