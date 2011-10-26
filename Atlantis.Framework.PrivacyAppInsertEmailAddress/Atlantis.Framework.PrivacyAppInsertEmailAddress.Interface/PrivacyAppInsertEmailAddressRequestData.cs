using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PrivacyAppInsertEmailAddress.Interface
{
  public class PrivacyAppInsertEmailAddressRequestData : RequestData
  {
    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(5);
    private string _emailAddress = string.Empty;

    public PrivacyAppInsertEmailAddressRequestData(
      string shopperId,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
    }

    public string EmailAddress
    {
      get { return _emailAddress; }
      set { _emailAddress = value; }
    }


    public override string GetCacheMD5()
    {
      throw new NotImplementedException("PrivacyAppInsertEmailAddress is not a cacheable request.");
    }

    public override string ToXML()
    {
      return string.Empty;
    }

  }
}
