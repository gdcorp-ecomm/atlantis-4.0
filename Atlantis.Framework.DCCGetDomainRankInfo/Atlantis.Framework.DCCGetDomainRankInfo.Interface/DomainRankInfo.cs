using System;

namespace Atlantis.Framework.DCCGetDomainRankInfo.Interface
{
  public class DomainRankInfo
  {
    public enum RankType
    {
      Red,
      Yellow,
      Green
    }

    public enum ProcessingStatusType
    {
      Success,
      NotFound
    }

    public ProcessingStatusType ProcessingStatus { get; set; }
    public string ShopperId { get; set; }
    public string DomainName { get; set; }
    public RankType Rank { get; set; }
    public DateTime DateScored { get; set; }
    public string DomainDiagnosticUrl { get; set; }
    public string LinkText { get; set; }
  }
}
