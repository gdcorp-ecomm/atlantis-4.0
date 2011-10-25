using Atlantis.Framework.MyaProduct.Interface;

namespace Atlantis.Framework.MyaProductSharedHosting.Interface
{
  public class SharedHostingPagingResult : IPagingResult
  {
    public int TotalNumberOfRecords { get; private set; }

    public int TotalNumberOfPages { get; private set; }

    internal SharedHostingPagingResult(int totalNumberOfRecords, int totalNumberOfPages)
    {
      TotalNumberOfRecords = totalNumberOfRecords;
      TotalNumberOfPages = totalNumberOfPages;
    }
  }
}
