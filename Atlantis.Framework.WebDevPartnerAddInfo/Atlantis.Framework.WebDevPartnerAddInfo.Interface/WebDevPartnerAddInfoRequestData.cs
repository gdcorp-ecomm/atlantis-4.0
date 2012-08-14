using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WebDevPartnerAddInfo.Interface
{
  public class WebDevPartnerAddInfoRequestData : RequestData
  {
    public WebDevPartnerAddInfoRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount, string privacyHashKey, string applicationType, string applicationState)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PrivacyHashKey = privacyHashKey;
      ApplicationType = applicationType;
      ApplicationState = applicationState;
    }

    public string PrivacyHashKey { get; set; }
    public string ApplicationType { get; set; }
    public string ApplicationState { get; set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("WebDevPartnerAddInfoRequestData is not a cacheable request.");
    }
  }
}
