using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrionAddAttribute.Interface
{
  public class OrionAddAttributeResponseData : IResponseData
  {
    public AtlantisException AtlantisException { get; private set; }
    public bool IsSuccess { get; private set; }
    public string Error { get; private set; }

    public OrionAddAttributeResponseData(int responseCode, string error)
    {
      IsSuccess = responseCode == 0;
      Error = error;
    }

    public OrionAddAttributeResponseData(AtlantisException exception)
    {
      AtlantisException = exception;
      IsSuccess = false;
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }
  }
}
