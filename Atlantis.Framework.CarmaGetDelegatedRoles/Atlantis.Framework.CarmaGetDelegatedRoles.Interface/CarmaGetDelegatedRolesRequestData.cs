using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaGetDelegatedRoles.Interface
{
    public class CarmaGetDelegatedRolesRequestData : RequestData
    {

      public int ApplicationId { get; set; }
      public int ResourceId { get; set; }
      public int ResourceType { get; set; }

      public CarmaGetDelegatedRolesRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int applicationId, int resourceId, int resourceType)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
      {
        ApplicationId = applicationId;
        ResourceId = resourceId;
        ResourceType = resourceType;
      }

      public override string GetCacheMD5()
      {
        throw new NotImplementedException("GetCacheMD5 not implemented in CarmaGetDelegatedRolesRequestData");
      }
    }
}
