using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetParentResourcesByAvailableCredits.Interface
{
  public class GetParentResourcesByAvailableCreditsResponseData : IResponseData
  {
    public string XML { get; set; }
    public string ToXML()
    {
      return XML;
    }

    public AtlantisException AtlException { get; set; }
    public AtlantisException GetException()
    {
      return AtlException;
    }
  }
}
