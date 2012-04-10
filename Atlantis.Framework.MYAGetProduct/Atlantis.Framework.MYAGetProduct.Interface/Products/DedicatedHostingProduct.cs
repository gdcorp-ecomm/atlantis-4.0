using System;
using System.Data;
using System.Globalization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MYAGetProduct.Interface.Products
{
  public class DedicatedHostingProduct : MyaProduct
  {
    public DedicatedHostingProduct() { }

    public override MyaProduct PopulateObjectFromDB(IDataReader dr, MYAGetProductRequestData myaProductRequestData, bool hasIsFreeColumn)
    {
      MyaProduct dhp = new DedicatedHostingProduct();

      dhp.BillingResourceId = Convert.ToInt32(dr["resource_id"], CultureInfo.CurrentCulture);
			dhp.ProductId = Convert.ToInt32(dr["pf_id"], CultureInfo.CurrentCulture);
			dhp.RenewalPfId = dr["renewal_pf_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["renewal_pf_id"], CultureInfo.CurrentCulture);
      dhp.IsPastDue = Convert.ToInt32(dr["isPastDue"], CultureInfo.CurrentCulture) == 0 ? false : true;			
      dhp.Namespace = dr["namespace"] == DBNull.Value ? null : dr["namespace"].ToString().Trim();
      dhp.LastExpirationDate = dr["last_expiration_date"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["last_expiration_date"], CultureInfo.CurrentCulture);

			// Expiration date should never be null;
			if (dr["expiration_date"] == DBNull.Value)
			{
        throw new AtlantisException(myaProductRequestData,
          "DedicatedHostingProductLite::PopulateObjectFromDB",
          "Billing expiration_date is null for this account. " + "ResourceID: " + dhp.BillingResourceId.ToString(CultureInfo.CurrentCulture),
          string.Empty);
			}
			else
      {
				dhp.ExpirationDate = Convert.ToDateTime(dr["expiration_date"]);
      }

			dhp.Credits = Convert.ToInt32(dr["billing_credits"]);
			dhp.CommonName = Convert.ToString(dr["commonName"]);
			dhp.OrionResourceId = Convert.ToString(dr["externalResourceID"]);
      dhp.RecurringPayment = dr["recurring_payment"] == DBNull.Value ? "" : Convert.ToString(dr["recurring_payment"], CultureInfo.CurrentCulture);

      dhp.AccountExpirationDate = dr["account_expiration_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["account_expiration_date"], CultureInfo.CurrentCulture);
      dhp.AutoRenewFlag = Convert.ToInt32(dr["autoRenewFlag"], CultureInfo.CurrentCulture) == 0 ? false : true;
      dhp.NumberOfPeriods = 0;
      dhp.PpShopperProfileId = dr["pp_shopperProfileID"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["pp_shopperProfileID"], CultureInfo.CurrentCulture);
      dhp.RecurringPayment = string.Empty;

      //Set non-nullable properties...
      dhp.Description = string.Empty;
      dhp.IsFree = false;

      return dhp;
    }
  }
}
