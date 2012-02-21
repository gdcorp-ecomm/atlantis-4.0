using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetShopperByEmailAddress.Interface
{
  public class ECCGetShopperByEmailAddressRequestData: RequestData
  {

    private string _emailAddress;
    public string EmailAddress
    {
      get
      {
        return _emailAddress;
      }
    }

    public ECCGetShopperByEmailAddressRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string emailAddress)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _emailAddress = emailAddress;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
