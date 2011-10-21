using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetCreditGroupSummary.Interface
{
  public class GetCreditGroupSummaryResponseData : IResponseData
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
