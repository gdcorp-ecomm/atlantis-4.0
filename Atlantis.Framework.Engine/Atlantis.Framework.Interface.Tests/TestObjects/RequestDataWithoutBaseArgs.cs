namespace Atlantis.Framework.Interface.Tests.TestObjects
{
  public class RequestDataWithoutBaseArgs : RequestData
  {
    public string MyArg { get; private set; }

    public RequestDataWithoutBaseArgs(string myArg)
    {
      MyArg = myArg;
    }
  }
}
