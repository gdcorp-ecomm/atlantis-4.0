using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresCentral.Interface
{
  public abstract class PCResponseDataBase : IResponseData
  {
    readonly PCResponse _responseData;

    protected PCResponseDataBase(PCResponse responseData)
    {
      _responseData = responseData;
    }

    public PCResponse Data
    {
      get { return _responseData; }
    }

    public string ToXML()
    {
      string result = null;
      if (_responseData != null)
      {
        result = _responseData.ToXML();
      }
      return result;
    }

    public AtlantisException GetException()
    {
      return null;
    }

  }
}
