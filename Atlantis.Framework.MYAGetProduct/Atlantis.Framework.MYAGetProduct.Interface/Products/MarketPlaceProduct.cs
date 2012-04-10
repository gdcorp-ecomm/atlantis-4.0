using System;
using System.Data;
using System.Globalization;
using System.Xml;

namespace Atlantis.Framework.MYAGetProduct.Interface.Products
{
  public class MarketPlaceProduct : MyaProduct
  {
    #region Properties

    private bool? _canRenewFreeProducts;

    public bool? CanRenewFreeProducts
    {
      get { return _canRenewFreeProducts; }
      set { _canRenewFreeProducts = value; }
    }
    #endregion

    public MarketPlaceProduct() { }

    public override MyaProduct PopulateObjectFromDB(IDataReader dr, MYAGetProductRequestData myaProductRequestData, bool hasIsFreeColumn)
    {
      int workerOutInt = 0;
      MyaProduct mppl = new MarketPlaceProduct();

      mppl = base.PopulateObjectFromDB(dr, myaProductRequestData, hasIsFreeColumn);

      mppl.CommonName = Convert.ToString(dr["commonName"], CultureInfo.CurrentCulture).Trim();
      mppl.ObsoleteResourceId = int.TryParse(dr["obsoleteResourceID"].ToString(), out workerOutInt) ? (int?)workerOutInt : (int?)Convert.ToInt32(dr["resource_id"]);
      mppl.IsFree = dr["isFree"] == DBNull.Value ? Convert.ToInt32(dr["isFree"]) == 1 : false;

      return mppl;
    }
  }
}
