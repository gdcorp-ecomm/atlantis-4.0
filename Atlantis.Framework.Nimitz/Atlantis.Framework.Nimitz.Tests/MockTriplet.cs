using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Nimitz.Tests
{
  public class MockTripletRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null; 

      string connectionString = NetConnect.LookupConnectInfo(oConfig);
      if (string.IsNullOrEmpty(connectionString))
      {
        throw new Exception("error");
      }
      else
      {
        result = new MockTripletResponseData(true);
      }

      return result;
    }

    #endregion
  }

  public class MockTripletRequestData : RequestData
  {
    public MockTripletRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) :
      base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }

  public class MockTripletResponseData : IResponseData
  {
    #region IResponseData Members

    private AtlantisException _ex;
    public bool Success { get; set; }

    public MockTripletResponseData(bool isSuccess)
    {
      Success = isSuccess;
    }

    public MockTripletResponseData(Exception ex, RequestData request)
    {
      Success = false;
      _ex = new AtlantisException(request, "MockTripletResponseData", ex.Message, ex.StackTrace, ex);
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }


}
