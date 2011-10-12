using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.GoodAsGoldReport.Interface.Abstract;

namespace Atlantis.Framework.GoodAsGoldReport.Interface.Concrete
{
  public class GAGPagingInfo: IPageInfo
  {
    public int PageSize {get;set;}

    public int CurrentPage { get; set; }

    public GAGPagingInfo()
    {
      PageSize = 5;
      CurrentPage = 1;
    }
  }
}
