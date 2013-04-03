using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainIsProtected.Interface
{
  public class DomainIsProtectedRequestData : RequestData
  {

    public int ResourceId { get; set; }

    public DomainIsProtectedRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount, int resourceId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ResourceId = resourceId;
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("DomainIsProtectedRequestData is not a cachable request.");
    }

    #endregion
  }
}
