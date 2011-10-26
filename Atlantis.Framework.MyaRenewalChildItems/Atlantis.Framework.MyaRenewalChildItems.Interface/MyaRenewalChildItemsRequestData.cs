using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaRenewalChildItems.Interface
{
  public class MyaRenewalChildItemsRequestData : RequestData
  {
    public int ResourceID { get; set; }

    public string Namespace { get; set; }

    public MyaRenewalChildItemsRequestData(
      string shopperID,
      string sourceURL,
      string orderID,
      string pathway,
      int pageCount,
      int resourceID, string nameSpace )
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      ResourceID = resourceID;
      Namespace = nameSpace;
    }

    public override string GetCacheMD5()
    {
      return string.Empty;
    }
  }
}
