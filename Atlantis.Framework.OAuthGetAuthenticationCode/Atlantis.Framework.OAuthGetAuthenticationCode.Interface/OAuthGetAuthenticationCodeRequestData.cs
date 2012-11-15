using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OAuthGetAuthenticationCode.Interface
{
  public class OAuthGetAuthenticationCodeRequestData: RequestData
  {
    public string PalmsID { get; private set; }
    public string BillingType { get; private set; }
    public string BillingNamespace { get; private set; }
    public string ResourceID { get; private set; }
    public string ResourceDescription { get; private set; }
    public string AccessList { get; private set; }

    public OAuthGetAuthenticationCodeRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, 
      string palmsId, string billingNamespace, string resourceId, string resourceDescription, string accessList)
      :base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PalmsID = palmsId;
      BillingNamespace = billingNamespace;
      ResourceID = resourceId;
      ResourceDescription = resourceDescription;
      AccessList = accessList;

      BillingType = "BILLING"; //hardcoded this per reqs.
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
