using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Entities.Interfaces
{
  public interface IWidget<T>
  {
    string Path { get; set; }
    //int ViewId { get; set; }
    string WidgetId { get; set; }
    T WidgetModel { get; set; }
    string ZoneId { get; set; }
  }
}
