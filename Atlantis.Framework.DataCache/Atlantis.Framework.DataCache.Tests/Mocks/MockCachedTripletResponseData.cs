using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DataCache.Tests.Mocks
{
  public class MockCachedTripletResponseData : IResponseData
  {
    public MockCachedTripletResponseData(string dataValue)
    {
      DataValue = dataValue;
    }

    public string DataValue { get; private set; }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return null;
    }

    public string ToXML()
    {
      return "<MockCachedTripletResponseData value='" + DataValue + "' />";
    }

    #endregion
  }
}
