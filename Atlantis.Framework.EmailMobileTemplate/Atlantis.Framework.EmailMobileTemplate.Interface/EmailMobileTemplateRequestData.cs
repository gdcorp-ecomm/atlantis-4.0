using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EmailMobileTemplate.Interface
{
  public class EmailMobileTemplateRequestData : RequestData
  {
    public int BounceBackEmailId { get; private set; }

    public string MessageGuid { get; private set; }

    public string IscCode { get; private set; }

    public int PrivateLabelId { get; private set; }

    public bool ShopperIdInLinkMatchesShopperCookie { get; set; }

    public EmailMobileTemplateRequestData(int bounceBackEmailId, string messageGuid, string iscCode, string sShopperID, int privateLabelId, string sSourceURL, string sOrderID, string sPathway, int iPageCount) : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      BounceBackEmailId = bounceBackEmailId;
      MessageGuid = messageGuid;
      IscCode = iscCode;
      PrivateLabelId = privateLabelId;
      ShopperIdInLinkMatchesShopperCookie = false;
      RequestTimeout = TimeSpan.FromSeconds(10);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("Email Mobile Template is not a cacheable request.");
    }
  }
}
