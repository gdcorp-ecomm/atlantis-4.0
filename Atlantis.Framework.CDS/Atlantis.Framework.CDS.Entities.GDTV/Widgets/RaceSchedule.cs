using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;


namespace Atlantis.Framework.CDS.Entities.GDTV.Widgets
{
  public class RaceSchedule : IWidgetModel
  {
    public RaceSchedule()
    {
      this.Schedules = new List<Schedule>();
    }
    public string Title { get; set; }
    public string RaceScheduleLink { get; set; }
    public IList<Schedule> Schedules { get; set; }
  }

  public class Schedule
  {
    public Schedule()
    {
      this.Races = new List<Race>();
    }
    public string Series { get; set; }
    public string DisplayName { get; set; }
    public string LongDisplayName { get; set; }
    public int DisplayOrder { get; set; }
    public string IconClass { get; set; }
    public string TableIcon { get; set; }
    public IList<Race> Races { get; set; }
  }

  public class Race
  {
    public string Date { get; set; }
    public string Event { get; set; }
    public string Car { get; set; }
    public string Network { get; set; }
  }
}
