using System;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class TLDPhaseDate
  {
    internal TLDPhaseDate(string phaseCode, DateTime startDate, DateTime endDate)
    {
      PhaseCode = phaseCode;
      StartDate = startDate;
      EndDate = endDate;
    }

    public string PhaseCode { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
  }
}
