using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommValidPaymentType.Interface
{
  public class EcommValidPaymentTypeRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(20);

    public string BasketType { get; set; }

    /// <summary>
    /// This is the transactional currency the cart has in the cart xml
    /// </summary>
    public string TransactionalCurrencyType { get; set; }

    public EcommValidPaymentTypeRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string basketType, string transactionalCurrencyType) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      BasketType = basketType;
      TransactionalCurrencyType = transactionalCurrencyType;
      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("EcommValidPaymentTypeRequestData is not a cacheable request.");
    }
  }
}
