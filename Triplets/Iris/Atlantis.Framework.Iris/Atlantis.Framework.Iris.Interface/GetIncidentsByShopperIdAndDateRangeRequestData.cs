using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Iris.Interface
{
  public class GetIncidentsByShopperIdAndDateRangeRequestData : RequestData
  {
    public GetIncidentsByShopperIdAndDateRangeRequestData(string shopperId, DateTime startDate, DateTime endDate)
    {
      StartDate = startDate;
      EndDate = endDate;
      ShopperId = shopperId;
    }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string ShopperId
    {
      get { return base.ShopperID; }
      set { base.ShopperID = value; }
    }
  }
}
