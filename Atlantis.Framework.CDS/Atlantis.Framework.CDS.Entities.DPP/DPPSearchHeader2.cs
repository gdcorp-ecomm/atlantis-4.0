﻿using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.DPP.Widgets
{
  public class DPPSearchHeader2 : IWidgetModel
  {
    public DPPSearchHeader2()
    {
    }

    public string HeaderText { get; set; }
    public string CSS { get; set; }
    public string JavaScript { get; set; }
    public string Markup { get; set; }
    public List<InnerControl> UserControls { get; set; }

    public class InnerControl
    {
      public string DPPUserControl { get; set; }
      public string QueryStringParameters { get; set; }
    }

  }


}