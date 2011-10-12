using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.GoodAsGoldReport.Interface;

namespace Atlantis.Framework.GoodAsGoldReport.Interface.Abstract
{
 
  public interface IPageInfo
  {
    int PageSize { get; set; }

    int CurrentPage { get; set; }   
  }
}
