using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ResourceIdGetByExtResource.Interface
{
  public class ResourceIdGetByExtResourceRequestData : RequestData
  {
    #region Properties

    public string ExternalResourceId { get; set; }
    public string OrionNamespace { get; set; }

    #endregion Properties

    public ResourceIdGetByExtResourceRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , string externalResourceId
      , string orionNamespace)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5d);
      ExternalResourceId = externalResourceId;
      OrionNamespace = orionNamespace;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in ResourceIdGetByExtResourceRequestData");
    }

  }
}
