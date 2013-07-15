using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommGiftCardStatement.Interface
{
  public class EcommGiftCardStatementRequestData : RequestData
  {

    public int ResourceId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public EcommGiftCardStatementRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount, int resourceId, DateTime? startDate = null, DateTime? endDate = null)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ResourceId = resourceId;
      StartDate = startDate ?? new DateTime(1990, 1, 1);
      EndDate = endDate ?? DateTime.Now;
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("EcommGiftCardStatementRequestData is not a cachable request.");
    }

    #endregion
  }
}
