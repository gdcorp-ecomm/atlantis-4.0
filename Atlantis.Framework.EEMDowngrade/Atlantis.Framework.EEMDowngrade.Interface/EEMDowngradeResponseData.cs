using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMDowngrade.Interface
{
  public class EEMDowngradeResponseData :IResponseData
  {
    public bool IsSuccessful { get; set; }
    public AtlantisException AtlantisException { get; set; }

    public EEMDowngradeResponseData()
    {
      IsSuccessful = true;
    }

    public EEMDowngradeResponseData(AtlantisException atlEx)
    {
      AtlantisException = atlEx;
      IsSuccessful = false;
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
