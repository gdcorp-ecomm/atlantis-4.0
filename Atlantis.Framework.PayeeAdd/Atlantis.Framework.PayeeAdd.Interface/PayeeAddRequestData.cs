using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PayeeProfileClass.Interface;

namespace Atlantis.Framework.PayeeAdd.Interface
{
  public class PayeeAddRequestData : RequestData
  {
    private PayeeProfile Payee { get; set; }

    public PayeeAddRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , PayeeProfile payee)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Payee = payee;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    #region Overridden Methods
    public override string ToXML()
    {
      return XmlBuilder.BuildAddPayeeXml(ShopperID, Payee);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in PayeeAddRequestData");
    }
    #endregion
  }
}
