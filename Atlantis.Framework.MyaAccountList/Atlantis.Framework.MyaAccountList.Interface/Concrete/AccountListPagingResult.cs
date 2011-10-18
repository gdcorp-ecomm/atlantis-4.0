using Atlantis.Framework.MyaAccountList.Interface.Abstract;

namespace Atlantis.Framework.MyaAccountList.Interface.Concrete
{
  public class AccountListPagingResult : IPageResult
  {
    public int TotalPages { get; set; }

    public int TotalRecords { get; set; }

    public AccountListPagingResult(int _totalPages, int _totalRecords)
    {
      TotalPages = _totalPages;
      TotalRecords = _totalRecords;
    }
  }
}
