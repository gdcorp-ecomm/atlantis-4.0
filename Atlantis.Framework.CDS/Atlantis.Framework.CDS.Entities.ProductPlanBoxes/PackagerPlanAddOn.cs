using System;
using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Widgets;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.ProductPlanBoxes
{
  public class PackagerPlanAddOn : ElementBase
  {
    public string InputLabel { get; set; }

    public string SelectType { get; set; }

    public string InputType { get; set; }

    public string AddOnName { get; set; }

    public bool Hide { get; set; }

    public int PackagerPlanAddOnType { get; set; }

    public string QuantityValues { get; set; }

    [JsonIgnore]
    private IList<int> _quantityList;
    [JsonIgnore]
    public IList<int> QuantityList
    {
      get
      {
        if (_quantityList == null)
        {
          string[] quantityValuesArray = QuantityValues.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
          _quantityList = new List<int>(quantityValuesArray.Length);
          foreach (string quantityValue in quantityValuesArray)
          {
            int quantity;
            if (int.TryParse(quantityValue, out quantity))
            {
              _quantityList.Add(quantity);
            }
          }
        }
        return _quantityList;
      }
    }
  }
}
