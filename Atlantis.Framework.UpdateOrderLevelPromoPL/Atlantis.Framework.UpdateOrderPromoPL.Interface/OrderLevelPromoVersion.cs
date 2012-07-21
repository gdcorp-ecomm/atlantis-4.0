using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.AddOrderLevelPromoPL.Interface;

namespace Atlantis.Framework.UpdateOrderPromoPL.Interface
{
  public class OrderLevelPromoVersion : OrderLevelPromo
  {
    public OrderLevelPromoVersion()
    {}

    public OrderLevelPromoVersion(string promoId, int versionId, string startDate, string endDate, bool isActive, string iscCode, string iscDescription)
      : base(promoId, startDate, endDate, isActive, iscCode, iscDescription)
    {
      this._versionId = versionId; 
    }

    new internal int ColumnType
    {
      get { return base.ColumnType; }
    }

    new internal int ServerGroupId
    {
      get { return base.ServerGroupId; }
    }

    private int _versionId;
    public int VersionId
    {
      get { return this._versionId; }
      set { this._versionId = value; }
    }

  }
}
