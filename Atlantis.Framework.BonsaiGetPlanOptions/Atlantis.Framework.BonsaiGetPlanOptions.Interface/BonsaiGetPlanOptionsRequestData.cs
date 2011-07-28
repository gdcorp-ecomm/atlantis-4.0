
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiGetPlanOptions.Interface
{
  public class BonsaiGetPlanOptionsRequestData : RequestData
  {
    public BonsaiGetPlanOptionsRequestData(string shopperId, string sourceUrl, string orderId,
                                           string pathway, int pageCount, string resourceId,
                                           string resourceType, string idType, int treeId,
                                           int privateLabelId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ResourceId = resourceId;
      ResourceType = resourceType;
      IdType = idType;
      TreeId = treeId;
      PrivateLabelId = privateLabelId;
      BonsaiGetAccountXmlRequestType = 74;
    }

    public override string GetCacheMD5()
    {
      return string.Empty;
    }

    public string ResourceId { get; set; }
    public string ResourceType { get; set; }
    public string IdType { get; set; }
    public int TreeId { get; set; }
    public int PrivateLabelId { get; set; }
    public int BonsaiGetAccountXmlRequestType { get; set; }
  }
}
