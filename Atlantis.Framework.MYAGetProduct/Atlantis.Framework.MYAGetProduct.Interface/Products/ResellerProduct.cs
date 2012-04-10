using System;
using System.Data;
using System.Globalization;
using Atlantis.Framework.MYAGetProduct.Interface.ProductHelper;

namespace Atlantis.Framework.MYAGetProduct.Interface.Products
{
  public class ResellerProduct : MyaProduct
  {
    public ResellerProduct() { }

    public override MyaProduct PopulateObjectFromDB(IDataReader dr, MYAGetProductRequestData myaProductRequestData, bool hasIsFreeColumn)
    {
      int workerOutInt = 0;
      MyaProduct rp = new ResellerProduct();

      rp.BillingResourceId = dr["recurring_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["recurring_id"], CultureInfo.CurrentCulture);
      rp.ProductId = Convert.ToInt32(dr["pf_id"], CultureInfo.CurrentCulture);
      rp.ProductTypeId = MyaProductTypeId.Reseller;
      rp.BundlePfId = dr["bundle_pf_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["bundle_pf_id"], CultureInfo.CurrentCulture);
      rp.RenewalPfId = -1;
      rp.ExpirationDate = DateTime.MaxValue;
      rp.AccountExpirationDate = dr["billing_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["billing_date"], CultureInfo.CurrentCulture);
      rp.LastExpirationDate = DateTime.MaxValue;
      rp.Credits = 0;
      rp.CommonName = Convert.ToString(dr["entityName"], CultureInfo.CurrentCulture).Trim();
      rp.OrionResourceId = string.Empty;
      rp.IsPastDue = false;
      rp.Namespace = string.Empty;
      rp.ParentBundleProductTypeId = dr["parent_bundle_product_typeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["parent_bundle_product_typeID"], CultureInfo.CurrentCulture);
      rp.PrivateLabelGroupType = (int?)Convert.ToInt32(dr["privateLabelResellerTypeID"]);
      rp.EntityId = int.TryParse(dr["entityID"].ToString(), out workerOutInt) ? (int?)workerOutInt : null;

      //ParentBundleId must default to zero
      rp.ParentBundleId = int.TryParse(dr["parent_bundle_id"].ToString(), out workerOutInt) ? (int?)workerOutInt : (int?)0;

      rp.ObsoleteResourceId = -1;
      rp.Description = Convert.ToString(dr["Description"], CultureInfo.CurrentCulture).Trim();
      rp.IsFree = false;

      rp.AutoRenewFlag = Convert.ToInt32(dr["autoRenewFlag"], CultureInfo.CurrentCulture) == 0 ? false : true;
      rp.NumberOfPeriods = 0;
      rp.PpShopperProfileId = dr["pp_shopperProfileID"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["pp_shopperProfileID"], CultureInfo.CurrentCulture);
      rp.RecurringPayment = string.Empty;

      return rp;
    }
  }
}
