using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Common;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class PodContainer : IWidgetModel
  {

    public PodContainer()
    {
      Widgets = new List<Widget<IWidgetModel>>();
    }

    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }
    public bool UseRoundedCorners { get; set; }
  }
}

  
