using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommProductAddOns.Interface
{
  public class EcommProductAddOnsRequestData : RequestData
  {
    public string OrionId { get; private set; }
    public string ResourceType { get; private set; }
    public string IdType { get; private set; }
    public int PrivateLabelId { get; private set; }

    /// <summary>
    /// This Request will return a list of AddOn productIds and renewal productIds
    /// </summary>
    /// <param name="shopperId"></param>
    /// <param name="sourceUrl"></param>
    /// <param name="orderId"></param>
    /// <param name="pathway"></param>
    /// <param name="pageCount"></param>
    /// <param name="privateLabelId"></param>
    /// <param name="orionId"></param>
    /// <param name="resourceType">Billing, Orion or Bonsai Namespace value (EXEC [gdshop_product_typeGetNamespaceList_sp])</param>
    /// <param name="idType">Allowed values: Billing (default), Orion or Bonsai</param>
    public EcommProductAddOnsRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int privateLabelId
      , string orionId
      , string resourceType
      , string idType = "Billing")
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      OrionId = orionId;
      ResourceType = resourceType;
      IdType = idType;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EcommProductAddOnsRequestData");     
    }
  }
}
