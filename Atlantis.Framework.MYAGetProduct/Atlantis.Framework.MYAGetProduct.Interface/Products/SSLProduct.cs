using System;
using System.Data;
using System.Globalization;

namespace Atlantis.Framework.MYAGetProduct.Interface.Products
{
  public class SSLProduct : MyaProduct
  {
    public SSLProduct() { }

    public override MyaProduct PopulateObjectFromDB(IDataReader dr, MYAGetProductRequestData myaProductRequestData, bool hasIsFreeColumn)
    {
      MyaProduct ssl = new SSLProduct();
      ssl.ProductId = Convert.ToInt32(dr["pf_id"]);
      ssl.ProductTypeId = myaProductRequestData.ProductTypeId;
      ssl.BundlePfId = (int?)0;
      ssl.BillingResourceId = Convert.ToInt32(dr["resource_id"]);
      ssl.RenewalPfId = dr["renewal_pf_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["renewal_pf_id"], CultureInfo.CurrentCulture);
      if (dr["next_attempt_date"] != DBNull.Value)
        ssl.ExpirationDate = Convert.ToDateTime(dr["next_attempt_date"]);
      if (dr["expireDate"] != DBNull.Value)
        ssl.AccountExpirationDate = Convert.ToDateTime(dr["expireDate"]);
      if (dr["commonName"] != DBNull.Value)
        ssl.CommonName = Convert.ToString(dr["commonName"]);
      if (dr["parent_bundle_id"] != DBNull.Value)
      {
        int tmp = 0;
        if (!int.TryParse(dr["parent_bundle_id"].ToString(), out tmp))
          tmp = -1;
        ssl.ParentBundleId = (int?)tmp;
      }
      ssl.LastExpirationDate = dr["last_expiration_date"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["last_expiration_date"], CultureInfo.CurrentCulture);
      ssl.Description = string.Empty;
      ssl.Credits = 0;
      ssl.OrionResourceId = Convert.ToString(dr["externalResourceID"]);
      ssl.IsPastDue = false;
      ssl.Namespace = MyaProduct.NameSpace.SSLCert.ToString();
      ssl.Namespace = dr["namespace"] == DBNull.Value ? null : dr["namespace"].ToString().Trim();
      ssl.ObsoleteResourceId = 0;
      ssl.IsFree = false;
      ssl.AutoRenewFlag = Convert.ToInt32(dr["autoRenewFlag"], CultureInfo.CurrentCulture) == 0 ? false : true;
      ssl.NumberOfPeriods = 0;
      ssl.PpShopperProfileId = dr["pp_shopperProfileID"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["pp_shopperProfileID"], CultureInfo.CurrentCulture);
      ssl.RecurringPayment = string.Empty;

      return ssl;
    }
  }
}
