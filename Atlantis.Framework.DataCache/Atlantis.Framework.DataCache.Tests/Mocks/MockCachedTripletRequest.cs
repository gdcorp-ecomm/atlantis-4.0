using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DataCache.Tests.Mocks
{
  public class MockCachedTripletRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MockCachedTripletRequestData request = requestData as MockCachedTripletRequestData;
      MockCachedTripletResponseData result = new MockCachedTripletResponseData(request.RequestValue + ".OUTPUT");
      return result;
    }

    #endregion
  }
}
