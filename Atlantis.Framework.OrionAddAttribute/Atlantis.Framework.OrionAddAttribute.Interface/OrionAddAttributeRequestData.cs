using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.OrionAddAttribute.Interface.Types;

namespace Atlantis.Framework.OrionAddAttribute.Interface
{
  public class OrionAddAttributeRequestData : RequestData
  {
    public int PrivateLableId { get; private set; }
    public string OrionResourceId { get; private set; }
    public OrionAttribute Attribute { get; private set; }
    public string RequestedBy { get; private set; }

    public OrionAddAttributeRequestData(string shopperId,
                                        string sourceUrl,
                                        string orderId,
                                        string pathway,
                                        int pageCount,
                                        int privateLabelId,
                                        string orionResourceId,
                                        OrionAttribute attribute,
                                        string requestedBy)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      if (string.IsNullOrWhiteSpace(orionResourceId))
        throw new ArgumentOutOfRangeException("orionResourceId", "Orion AccountUID cannot be empty or null");
      Guid testValue;
      if (!Guid.TryParse(orionResourceId, out testValue))
        throw new ArgumentOutOfRangeException("orionResourceId", "orionResourceId is not a valid GUID string");
      if (attribute == null || attribute.Elements == null || attribute.Elements.Count == 0)
        throw new ArgumentException("attribute", "Orion Attribute and elements cannot be null or empty");

      PrivateLableId = privateLabelId;
      OrionResourceId = orionResourceId;
      Attribute = attribute;
      RequestedBy = requestedBy;
      RequestTimeout = TimeSpan.FromSeconds(10d);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in OrionAddAttributeRequestData");
    }
  }
}
