using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresCentral.Interface
{
  public abstract class PCResponseDataBase : IResponseData
  {
    AtlantisException _exception = null;
    PCResponse _responseData;

    public PCResponseDataBase(AtlantisException exception)
    {
      _exception = exception;
    }

    public PCResponseDataBase(PCResponse responseData)
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
      return _exception;
    }

  }
}
