using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommSwitchPaymentProfile.Interface
{
  public class EcommSwitchPaymentProfileRequestData : RequestData
  {
    public string ResourceId { get; private set; }
    public string ResourceType { get; private set; }
    public string IdType { get; private set; }
    public int NewPaymentProfileId { get; private set; }
    public string RequestorIp { get; private set; }

    public EcommSwitchPaymentProfileRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
                                                string resourceId, string resourceType, string idType,
                                                int newPaymentProfileId, string requestorIp)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ResourceId = resourceId;
      ResourceType = resourceType;
      IdType = idType;
      NewPaymentProfileId = newPaymentProfileId;
      RequestorIp = requestorIp;
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("EcommSwitchPaymentProfile is not a cacheable request.");
    }

  }
}
