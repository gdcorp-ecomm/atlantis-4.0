using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Common;

namespace Atlantis.Framework.CDS.Entities.GDTV.Widgets
{
  public class CelebFooter : IWidgetModel
  {
    public string BackgroundImage { get; set; }
    public int Height { get; set; }
    
    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }
  }
}
