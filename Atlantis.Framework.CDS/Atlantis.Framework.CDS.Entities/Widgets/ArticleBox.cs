using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class ArticleBox : IWidgetModel
  {
    public string HeaderText { get; set; }
    public string ArticleTextBottom { get; set; }
    public string ArticleTextTop { get; set; }
    public List<string> CurrentUnorderedList { get; set; }
    public string JavaScript { get; set; }
    public string CSS { get; set; }
    public string Markup { get; set; }
  }
}
