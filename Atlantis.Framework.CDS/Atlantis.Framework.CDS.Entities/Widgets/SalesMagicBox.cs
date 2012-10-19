using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  class SalesMagicBox : IWidgetModel
  {
    public SalesMagicBox()
      {
      }

      public string HeaderText { get; set; }
      public string CSS { get; set; }
      public string JavaScript { get; set; }
      public string Markup { get; set; }
  }
}
