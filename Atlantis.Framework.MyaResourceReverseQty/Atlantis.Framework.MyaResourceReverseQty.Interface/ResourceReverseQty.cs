using System;

namespace Atlantis.Framework.MyaResourceReverseQty.Interface
{
  public class ResourceReverseQty
  {
    public string OrderId { get; private set; }
    public int RowId { get; private set; }
    public int CanBeReversedQuantity { get; private set; }

    public ResourceReverseQty()
    { 
    }

    public ResourceReverseQty(string orderId, int rowId, int canBeReversedQuantity)
    {
      OrderId = orderId;
      RowId = rowId;
      CanBeReversedQuantity = canBeReversedQuantity;
    }
  }
}
