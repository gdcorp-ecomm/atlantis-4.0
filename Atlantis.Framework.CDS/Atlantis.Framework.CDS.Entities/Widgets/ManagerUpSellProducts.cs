using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using System.ComponentModel;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class ManagerUpSellProducts : IWidgetModel
  {
    public ManagerUpSellProducts()
    {
      ProductsToDisplay = string.Empty;
    }

    [DisplayName("Products To Display")]
    public string ProductsToDisplay { get; set; }
    [DisplayName("Include Form")]
    public bool IncludeForm { get; set; }
  }
}
