using System;
using System.Data;
using System.Globalization;
using Atlantis.Framework.MYAGetProduct.Interface.ProductHelper;

namespace Atlantis.Framework.MYAGetProduct.Interface.Products
{
  public class TrafficBlazerProduct : MyaProduct
  {
    public TrafficBlazerProduct() { }

    public override MyaProduct PopulateObjectFromDB(IDataReader dr, MYAGetProductRequestData myaProductRequestData, bool hasIsFreeColumn)
    {
      MyaProduct tbp = new TrafficBlazerProduct();

      tbp.BillingResourceId = Convert.ToInt32(dr["recurring_id"], CultureInfo.CurrentCulture);
      tbp.UserWebsiteId = dr["userwebsite_id"] == DBNull.Value ? -1 : Convert.ToInt32(dr["userwebsite_id"]);
      tbp.ProductId = 0;

      tbp.ProductTypeId = MyaProductTypeId.TrafficBlazer; 
      tbp.BundlePfId = 0;

      tbp.RenewalPfId = -1;

      tbp.ExpirationDate = DateTime.MaxValue;
      tbp.AccountExpirationDate = DateTime.MaxValue;

      if (dr["submittaldate"] == DBNull.Value)
      {
        tbp.DateSubmitted = DateTime.MinValue;
      }
      else
        tbp.DateSubmitted = Convert.ToDateTime(dr["submittaldate"], CultureInfo.CurrentCulture);

      tbp.Credits = 0;
      tbp.CommonName = dr["websiteurl"] == DBNull.Value ? "" : dr["websiteurl"].ToString().Trim();
      tbp.OrionResourceId = string.Empty;
      tbp.IsPastDue = false;
      tbp.Namespace = MyaProduct.NameSpace.trafblazer.ToString();

      tbp.ParentBundleId = 0;
      tbp.ObsoleteResourceId = 0;

      tbp.Description = string.Empty;

      tbp.IsFree = false;

      //TODO: Uncomment when proc is updated
      //tbp.AutoRenewFlag = Convert.ToInt32(dr["autoRenewFlag"], CultureInfo.CurrentCulture) == 0 ? false : true;
      tbp.NumberOfPeriods = 0;
      //tbp.PpShopperProfileId = dr["pp_shopperProfileID"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["pp_shopperProfileID"], CultureInfo.CurrentCulture);
      tbp.RecurringPayment = string.Empty;

      return tbp;
    }
  }
}
