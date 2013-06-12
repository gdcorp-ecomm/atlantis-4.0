using Atlantis.Framework.Interface;
using System;
using System.Threading;

namespace Atlantis.Framework.Engine.Tests.MockTriplet
{
  public class AsyncTripletRequest : IAsyncRequest
  {
    public IAsyncResult BeginHandleRequest(RequestData requestData, ConfigElement config, AsyncCallback callback, object state)
    {
      string returnValue = ((TestTripletRequestData)requestData).ResultValue;

      if ("beginerror" == returnValue)
      {
        throw new ApplicationException("beginerror");
      }

      if ("beginerroratlantis" == returnValue)
      {
        throw new AtlantisException(requestData, string.Empty, "beginerror", string.Empty);
      }

      Func<string, string> func;

      if ("duringerror" == returnValue)
      {
        func = (a) => { throw new NotImplementedException("duringerror"); };
      }
      else if ("threadabort" == returnValue)
      {
        func = (a) => { Thread.CurrentThread.Abort(); return a; };
      }
      else
      {
        func = (a) => { return a; };
      }

      if ("invalidstate" == returnValue)
      {
        return func.BeginInvoke(returnValue, callback, "invalidstateobject");
      }
      else
      {
        AsyncState asyncState = new AsyncState(requestData, config, func, state);
        return func.BeginInvoke(returnValue, callback, asyncState);
      }
    }

    public IResponseData EndHandleRequest(IAsyncResult asyncResult)
    {
      AsyncState asyncState = (AsyncState)asyncResult.AsyncState;
      Func<string, string> func = (Func<string, string>)asyncState.Request;
      string result = func.EndInvoke(asyncResult);

      if ("enderroratlantis" == result)
      {
        throw new AtlantisException(asyncState.RequestData, string.Empty, "enderror", string.Empty);
      }

      if ("null" == result)
      {
        return null;
      }

      return new TestTripletResponseData(result);
    }
 
  }
}
