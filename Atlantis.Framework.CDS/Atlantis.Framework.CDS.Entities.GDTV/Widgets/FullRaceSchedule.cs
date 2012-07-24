using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

  namespace Atlantis.Framework.CDS.Entities.GDTV.Widgets
  {
    public class FullRaceSchedule : IWidgetModel
    {
      public FullRaceSchedule()
      {
        this.Schedules = new List<Schedule>();
      }
      public string Title { get; set; }
      public IList<Schedule> Schedules { get; set; }
    }    
  }
