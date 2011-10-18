using Atlantis.Framework.MyaAccountList.Interface.Abstract;

namespace Atlantis.Framework.MyaAccountList.Interface.Concrete
{
  public class AccountListPagingInfo: IPageInfo
  {
    public int PageSize {get;set;}

    public int CurrentPage { get; set; }

    public AccountListPagingInfo()
    {
      PageSize = 5;
      CurrentPage = 1;
    }
  }
}
