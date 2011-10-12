using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.GoodAsGoldReport.Interface.Abstract
{
 
  public interface IPageResult
  {
    int TotalPages { get; set; }

    int TotalRecords { get; set; }
  }
}
