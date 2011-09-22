using System;
using Atlantis.Framework.Ecc.Interface.Enums;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetEmailAddressesForDomain.Interface
{
  public class ECCGetEmailAddressesForDomainRequestData: RequestData 
  {
    public ECCGetEmailAddressesForDomainRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, bool activeOnly, EmailTypes accountType, string domainName, int privateLabelId) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      ActiveOnly = activeOnly;
      AccountType = accountType;
      DomainName = domainName;
      PrivateLabelId = privateLabelId;
    }

    public int PrivateLabelId { get; set; }

    public string DomainName { get; set; }

    public EmailTypes AccountType { get; set; }

    public bool ActiveOnly { get; set; }

    public override string GetCacheMD5()
    {

      throw new Exception("ECCGetEmailAddressesForDomain is not a cacheable request.");
    }
  }
}
