using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Engine.Tests.MockTriplet
{
  public class TestTripletRequestData : RequestData
  {
    public string ResultValue { get; private set; }

    public TestTripletRequestData(string resultValue)
    {
      ResultValue = resultValue;
    }
  }
}
