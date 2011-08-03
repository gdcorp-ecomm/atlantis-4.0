
namespace Atlantis.Framework.MyaProduct.Interface
{
  public interface IPagingInfo
  {
    bool ReturnAll { get; set; }

    int CurrentPage { get; set; }

    int RowsPerPage { get; set; }
  }
}
