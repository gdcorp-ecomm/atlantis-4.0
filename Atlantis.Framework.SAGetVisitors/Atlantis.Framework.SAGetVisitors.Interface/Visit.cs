using System;

namespace Atlantis.Framework.SAGetVisitors.Interface
{
  public class Visit
  {
    public Visit (string statDate, string uniqueVisit, string visit)
    {
      DateTime tempDate;
      float tempVisit, tempUnique;

      if (DateTime.TryParse(statDate, out tempDate))
      {
        StatsDate = tempDate;
      }
      else
      {
        StatsDate = DateTime.MinValue;
      }

      if (float.TryParse(uniqueVisit, out tempUnique))
      {
        UniqueVisitors = tempUnique;
      }
      else
      {
        UniqueVisitors = -1;
      }

      if (float.TryParse(uniqueVisit, out tempVisit))
      {
        UniqueVisitors = tempVisit;
      }
      else
      {
        UniqueVisitors = -1;
      }

    }

    public DateTime StatsDate { get; set; }
    public float UniqueVisitors { get; set; }
    public float Visitors { get; set; }
  }
}
