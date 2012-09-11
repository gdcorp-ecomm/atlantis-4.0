using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.EcommInstorePending.Interface
{
  public class EcommInstorePendingRequestData : RequestData
  {
    public string TransactionalCurrencyType { get; set; }

    public EcommInstorePendingRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string transacationalCurrencyType)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      TransactionalCurrencyType = transacationalCurrencyType;
    }

    public override string ToXML()
    {
      XElement xml = new XElement("EcommInstorePendingRequestData",
        new XAttribute("shopperid", ShopperID),
        new XAttribute("transactionalcurrencytype", TransactionalCurrencyType));
      return xml.ToString();
    }
    public override string GetCacheMD5()
    {
      throw new NotImplementedException("EcommInstorePending is not a cacheable request.");
    }
  }
}
