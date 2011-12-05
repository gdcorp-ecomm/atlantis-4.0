using Atlantis.Framework.MyaProduct.Interface;

namespace Atlantis.Framework.MyaProductCashParking.Interface
{
  public class CashParkingPagingResult : IPagingResult
  {
    public int TotalNumberOfRecords { get; private set; }

    public int TotalNumberOfPages { get; private set; }

    internal CashParkingPagingResult(int totalNumberOfRecords, int totalNumberOfPages)
    {
      TotalNumberOfRecords = totalNumberOfRecords;
      TotalNumberOfPages = totalNumberOfPages;
    }
  }
}
