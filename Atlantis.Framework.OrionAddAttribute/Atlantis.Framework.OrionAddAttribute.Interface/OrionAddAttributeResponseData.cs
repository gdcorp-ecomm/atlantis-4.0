using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrionAddAttribute.Interface
{
  public class OrionAddAttributeResponseData : IResponseData
  {
    public string AttributeUid { get; private set; }
    public AtlantisException AtlantisException { get; private set; }
    public bool IsSuccess { get; private set; }
    public string Error { get; private set; }
    public int ResponseCode { get; private set; }

    public OrionAddAttributeResponseData(int responseCode, string attributeUid, string error)
    {
      AttributeUid = attributeUid;
      IsSuccess = responseCode == 0 && !string.IsNullOrEmpty(attributeUid);
      Error = error;
      ResponseCode = responseCode;
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
