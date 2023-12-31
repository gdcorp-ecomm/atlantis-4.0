﻿using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.Radios.Widgets
{
  public class RadiosContainer : IWidgetModel
  {
    public string Heading { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }

  }
}
