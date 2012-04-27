using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class ManagerUpSellProducts : IWidgetModel
  {
    public ManagerUpSellProducts()
    {
      ProductsToDisplay = string.Empty;
    }

    public string ProductsToDisplay { get; set; }
    public bool IncludeForm { get; set; }
  }
}
