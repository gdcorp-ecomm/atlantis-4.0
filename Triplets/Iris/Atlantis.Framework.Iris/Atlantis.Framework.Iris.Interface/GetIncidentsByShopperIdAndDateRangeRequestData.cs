using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Iris.Interface
{
  public class GetIncidentsByShopperIdAndDateRangeRequestData : RequestData
  {
    public GetIncidentsByShopperIdAndDateRangeRequestData(DateTime startDate, DateTime endDate)
    {
      StartDate = startDate;
      EndDate = endDate;
    }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
  }
}
