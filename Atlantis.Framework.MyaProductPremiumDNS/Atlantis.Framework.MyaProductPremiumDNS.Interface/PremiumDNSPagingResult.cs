using Atlantis.Framework.MyaProduct.Interface;

namespace Atlantis.Framework.MyaProductPremiumDNS.Interface
{
  public class PremiumDNSPagingResult : IPagingResult
  {
    public int TotalNumberOfRecords { get; private set; }

    public int TotalNumberOfPages { get; private set; }

    internal PremiumDNSPagingResult(int totalNumberOfRecords, int totalNumberOfPages)
    {
      TotalNumberOfRecords = totalNumberOfRecords;
      TotalNumberOfPages = totalNumberOfPages;
    }
  }
}
