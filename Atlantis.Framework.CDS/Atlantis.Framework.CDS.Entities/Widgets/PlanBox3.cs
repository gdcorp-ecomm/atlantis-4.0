using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class PlanBox3 : PlanBox2, IWidgetModel
  {
    public string AbovePlanDetailsCustomContent { get; set; }

    [RegularExpression(@"^(\d+,)*\d+$", ErrorMessage="Custom quantities must be a comma-separated list of nonnegative integers")]
    public string CustomQuantities { get; set; }

    private int[] _customQuantitiesList;
    public int[] CustomQuantitiesList
    {
      get
      {
        if (_customQuantitiesList == null)
        {
          if (!string.IsNullOrEmpty(CustomQuantities))
          {
            string[] temp = CustomQuantities.Split(new char[] { ',' });
            _customQuantitiesList = Array.ConvertAll(temp, Convert.ToInt32);
          }
        }
        return _customQuantitiesList;
      }
    }
  }
}
