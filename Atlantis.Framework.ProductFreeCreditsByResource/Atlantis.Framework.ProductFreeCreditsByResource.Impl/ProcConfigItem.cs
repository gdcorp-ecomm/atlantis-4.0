using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.ProductFreeCreditsByResource.Impl
{
  class ProcConfigItem
  {
    //db columns: gdshop_product_typeId, freeProductByResourceGetProc
    public int ProductTypeId { get; set; }
    public string StoredProcedure { get; set; }

    public ProcConfigItem(int productTypeId, string storedProcedure)
    {
      ProductTypeId = productTypeId;
      StoredProcedure = storedProcedure;
    }
  }
}
