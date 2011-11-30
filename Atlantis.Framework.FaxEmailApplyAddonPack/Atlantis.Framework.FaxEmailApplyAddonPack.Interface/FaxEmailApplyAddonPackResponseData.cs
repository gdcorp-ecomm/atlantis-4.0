using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FaxEmailApplyAddonPack.Interface
{
  public class FaxEmailApplyAddonPackResponseData : IResponseData
  {
    public AtlantisException AtlantisException { get; private set; }
    public bool IsSuccess { get; private set; }
    public string Error { get; private set; }

    public FaxEmailApplyAddonPackResponseData(int responseCode, string error)
    {
      IsSuccess = responseCode == 0;
      Error = error;
    }

    public FaxEmailApplyAddonPackResponseData(int responseCode, string error, AtlantisException exception)
    {
      IsSuccess = false;
      Error = error;
      AtlantisException = exception;
    }

    public FaxEmailApplyAddonPackResponseData(AtlantisException exception)
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
