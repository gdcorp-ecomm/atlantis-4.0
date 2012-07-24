using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Common
{
  public class Widget<T> : IWidget<T>
  {
    public string Path { get; set; }
    public string WidgetId { get; set; }
    public T WidgetModel { get; set; }
    public string ZoneId { get; set; }
  }
}
