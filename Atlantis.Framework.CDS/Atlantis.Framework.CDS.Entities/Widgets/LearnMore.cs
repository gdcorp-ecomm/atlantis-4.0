using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class LearnMore : IWidgetModel
  {
    public string Title { get; set; }
    public List<string> ListItems { get; set; }
  }
}
