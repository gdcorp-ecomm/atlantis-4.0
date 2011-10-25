using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaRenewalBundleItems.Interface
{
  public class MyaRenewalBundleItemsRequestData : RequestData
  {
    public int ResourceID { get; set; }

    public MyaRenewalBundleItemsRequestData(string shopperID,
                                            string sourceURL,
                                            string orderID,
                                            string pathway,
                                            int pageCount,
                                            int resourceID) : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      ResourceID = resourceID;
    }

    public override string GetCacheMD5()
    {
      return string.Empty;
    }
  }
}
