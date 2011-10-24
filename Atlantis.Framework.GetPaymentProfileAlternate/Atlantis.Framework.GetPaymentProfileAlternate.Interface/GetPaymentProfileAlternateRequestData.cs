using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetPaymentProfileAlternate.Interface
{
  public class GetPaymentProfileAlternateRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(20);

    public GetPaymentProfileAlternateRequestData(string shopperId,
                                             string sourceUrl,
                                             string orderId,
                                             string pathway,
                                             int pageCount
                                             )
      : this(shopperId, sourceUrl, orderId, pathway, pageCount, _defaultRequestTimeout)
    {
    }

    public GetPaymentProfileAlternateRequestData(string shopperId,
                                             string sourceUrl,
                                             string orderId,
                                             string pathway,
                                             int pageCount,
                                             TimeSpan requestTimeout)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = requestTimeout;
    }

    public override string GetCacheMD5()
    {
      string key = String.Concat("PaymentProfileAlternate", "|", "shopperid=", ShopperID);

      MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
      md5Provider.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
      byte[] md5Bytes = md5Provider.ComputeHash(stringBytes);
      return BitConverter.ToString(md5Bytes).Replace("-", string.Empty);
    }

  }
}
