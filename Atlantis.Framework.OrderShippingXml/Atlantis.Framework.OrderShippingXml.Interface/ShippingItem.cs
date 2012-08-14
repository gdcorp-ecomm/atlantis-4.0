
namespace Atlantis.Framework.OrderShippingXml.Interface
{
  public struct ShippingItem
  {
    public int RowId;
    public string Carrier;
    public string TrackingCode;
    public string EstDate;

    public ShippingItem(int rowId, string carrier, string trackingCode, string estDate)
    {
      RowId = rowId;
      Carrier = carrier;
      TrackingCode = trackingCode;
      EstDate = estDate;
    }
  }
}
