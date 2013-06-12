using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Engine.Tests.MockTriplet
{
  public class TestTripletResponseData : IResponseData
  {
    public string ResultData { get; private set; }
    private AtlantisException _exception = null;

    public TestTripletResponseData(string result)
    {
      ResultData = result;

      if ("responseerror" == result)
      {
        _exception = new AtlantisException("AsyncTripletResponseData.Constructor", "0", "responseerror", string.Empty, null, null);
      }
    }

    public string ToXML()
    {
      return "<result>" + ResultData + "</result>";
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
