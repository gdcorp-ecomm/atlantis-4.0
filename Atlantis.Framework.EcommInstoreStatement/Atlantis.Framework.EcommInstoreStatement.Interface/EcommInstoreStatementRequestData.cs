using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInstoreStatement.Interface
{
  public class EcommInstoreStatementRequestData : RequestData
  {

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Currency { get; set; }

    public EcommInstoreStatementRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, DateTime startDate, DateTime endDate, string currency=null)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      StartDate = startDate;
      EndDate = endDate;
      Currency = currency;
    }


    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  
  }
}
