using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.PromoOrderLevelCreate.Interface;

namespace Atlantis.Framework.PromoOrderLevelUpdate.Interface
{
  public class OrderLevelPromoVersion : OrderLevelPromo
  {
    public OrderLevelPromoVersion()
    {}

    public OrderLevelPromoVersion(string promoId, string startDate, string endDate, bool isActive, string iscCode, string iscDescription)
      : base(promoId, startDate, endDate, isActive, iscCode, iscDescription) { }

    new internal int ColumnType
    {
      get { return base.ColumnType; }
    }

    new internal int ServerGroupId
    {
      get { return base.ServerGroupId; }
    }

    private string _versionId;
    public string VersionId
    {
      get { return this._versionId; }
      set { this._versionId = value; }
    }

  }
}
