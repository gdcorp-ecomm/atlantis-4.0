using System;
using System.Data;
using System.Globalization;

namespace Atlantis.Framework.MYAGetProduct.Interface.Products
{
  public class EEMProduct : MyaProduct
  {

    #region Constructors

    public EEMProduct() { }

    #endregion

    #region Methods

    public override MyaProduct PopulateObjectFromDB(IDataReader dr, MYAGetProductRequestData myaProductRequestData, bool hasIsFreeColumn)
    {
      MyaProduct eem = new EEMProduct();

      eem.BillingResourceId = dr["CampaignBlazerID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CampaignBlazerID"], CultureInfo.CurrentCulture);
      eem.ProductId = Convert.ToInt32(dr["active_pfid"], CultureInfo.CurrentCulture);
      eem.ProductTypeId = myaProductRequestData.ProductTypeId;
      eem.CustomerId = dr["CustomerID"] == DBNull.Value ? -1 : Convert.ToInt32(dr["CustomerID"], CultureInfo.CurrentCulture);
      eem.BundlePfId = dr["bundle_pf_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["bundle_pf_id"], CultureInfo.CurrentCulture);
      eem.RenewalPfId = dr["renewal_pf_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["renewal_pf_id"], CultureInfo.CurrentCulture);
      eem.ExpirationDate = dr["billing_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["billing_date"], CultureInfo.CurrentCulture);
      eem.AccountExpirationDate = eem.ExpirationDate;
      eem.LastExpirationDate = DateTime.MaxValue;
      eem.Credits = 0;
      eem.CommonName = dr["CustomerID"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CustomerID"]);
      eem.OrionResourceId = eem.CustomerId.ToString();
      eem.IsPastDue = Convert.ToInt32(dr["isPastDue"], CultureInfo.CurrentCulture) == 1 ? true : false;
      eem.Namespace = dr["status"] == DBNull.Value ? "" : Convert.ToString(dr["status"], CultureInfo.CurrentCulture);
      eem.PendingQuota = dr["pending_quota"] == DBNull.Value ? 0 : Convert.ToInt32(dr["pending_quota"], CultureInfo.CurrentCulture);
      eem.ActiveQuota = dr["active_quota"] == DBNull.Value ? 0 : Convert.ToInt32(dr["active_quota"], CultureInfo.CurrentCulture);
      eem.AddOnQuota = dr["addon_quota"] == DBNull.Value ? 0 : Convert.ToInt32(dr["addon_quota"], CultureInfo.CurrentCulture);
      eem.EntityId = dr["rowid"] == DBNull.Value ? 0 : Convert.ToInt32(dr["rowid"], CultureInfo.CurrentCulture);
      eem.ParentBundleId = dr["parent_bundle_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["parent_bundle_id"], CultureInfo.CurrentCulture);
      eem.ParentBundleProductTypeId = dr["parent_bundle_product_typeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["parent_bundle_product_typeID"], CultureInfo.CurrentCulture);
      eem.RecurringPayment = dr["recurring_payment"] == DBNull.Value ? "" : Convert.ToString(dr["recurring_payment"], CultureInfo.CurrentCulture);
      eem.ObsoleteResourceId = -1;
      eem.Description = string.Empty;
      eem.StartDate = dr["start_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["start_date"], CultureInfo.CurrentCulture);
      // EEM doesn't have a isFree column 
      if (eem.ProductId != Convert.ToInt32(dr["pending_pfid"]))
      {
        eem.Namespace = "DowngradePending";
      }

      //TODO: Uncomment when Proc is updated!
      //eem.AutoRenewFlag = Convert.ToInt32(dr["autoRenewFlag"], CultureInfo.CurrentCulture) == 0 ? false : true;
      eem.NumberOfPeriods = 0;
      //eem.PpShopperProfileId = dr["pp_shopperProfileID"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["pp_shopperProfileID"], CultureInfo.CurrentCulture);
      eem.RecurringPayment = string.Empty;

      // and allows renewals of free accounts, so we can set isFree to false.
      eem.IsFree = false;

      return eem;
    }
    #endregion
  }
}
