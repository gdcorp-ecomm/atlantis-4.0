using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PayeeProfileClass.Interface;

namespace Atlantis.Framework.PayeeUpdate.Interface
{
  public class PayeeUpdateRequestData : RequestData
  {
    private PayeeProfile OriginalPayee { get; set; }
    private PayeeProfile UpdatedPayee { get; set; }

    public PayeeUpdateRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , PayeeProfile originalPayee
      , PayeeProfile updatedPayee)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      OriginalPayee = originalPayee;
      UpdatedPayee = updatedPayee;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    #region Overridden Methods
    public override string ToXML()
    {
      return XmlBuilder.BuildUpdatePayeeXml(ShopperID, OriginalPayee, UpdatedPayee);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in PayeeUpdateRequestData");
    }
    #endregion
  }
}
