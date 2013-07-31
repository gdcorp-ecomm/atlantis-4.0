using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Brand.Interface
{
  public class CompanyNameRequestData : RequestData
  {
    public int ContextId { get; private set; }
    public int PrivateLabelId { get; private set; }

    public CompanyNameRequestData(int contextId, int privateLabelId)
    {
      ContextId = contextId;
      PrivateLabelId = privateLabelId;
    }

    public override string GetCacheMD5()
    {
      return "companyNames";
    }

    public override string ToXML()
    {
      return "<companyNames />";
    }
  }
}