
namespace Atlantis.Framework.MyaProduct.Interface
{
  public interface IPagingResult
  {
    int TotalNumberOfRecords { get; }

    int TotalNumberOfPages { get; }
  }
}
