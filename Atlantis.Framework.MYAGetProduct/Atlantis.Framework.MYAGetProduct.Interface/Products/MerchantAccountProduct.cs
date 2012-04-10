using System;
using System.Globalization;
using System.Xml;
using Atlantis.Framework.MYAGetProduct.Interface.ProductHelper;

namespace Atlantis.Framework.MYAGetProduct.Interface.Products
{
  public class MerchantAccountProduct : MyaProduct
  {
    public MerchantAccountProduct() { }

    public override MyaProduct PopulateObjectFromDB(System.Data.IDataReader dr, MYAGetProductRequestData myaProductRequestData, bool hasIsFreeColumn)
    {
      MyaProduct map = new MerchantAccountProduct();

      map.MerchantAccountId = dr["merchantAccountID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["merchantAccountID"], CultureInfo.CurrentCulture);
      map.OrderId = dr["order_id"] == DBNull.Value ? string.Empty : Convert.ToString(dr["order_id"], CultureInfo.CurrentCulture);
      map.CommonName = Convert.ToString(dr["name"], CultureInfo.CurrentCulture).Trim();
      map.CreateDate = dr["createDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["createDate"], CultureInfo.CurrentCulture);
      map.AuthenticationGUID = Convert.ToString(dr["authenticationGUID"], CultureInfo.CurrentCulture).Trim();
      map.ApplicationDescription = Convert.ToString(dr["applicationDescription"], CultureInfo.CurrentCulture).Trim();
      map.SupportPhone = Convert.ToString(dr["supportPhone"], CultureInfo.CurrentCulture).Trim();
      map.ReferralId = dr["referralID"] == DBNull.Value ? "" : Convert.ToString(dr["referralID"], CultureInfo.CurrentCulture);

      map.ProductId = Convert.ToInt32(dr["pf_id"], CultureInfo.CurrentCulture);
      map.ProductTypeId = MyaProductTypeId.MerchantAccount;
      map.RenewalPfId = -1;
      map.OrionResourceId = string.Empty;
      map.IsPastDue = false;
      map.Namespace = string.Empty;
      map.ObsoleteResourceId = -1;
      map.Description = Convert.ToString(dr["applicationDescription"], CultureInfo.CurrentCulture).Trim();
      map.IsFree = false;

      //TODO: Uncomment when proc is updated
      //map.AutoRenewFlag = Convert.ToInt32(dr["autoRenewFlag"], CultureInfo.CurrentCulture) == 0 ? false : true;
      map.NumberOfPeriods = 0;
      //map.PpShopperProfileId = dr["pp_shopperProfileID"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["pp_shopperProfileID"], CultureInfo.CurrentCulture);
      map.RecurringPayment = string.Empty;

      //Setting non-nullable values
      map.AccountExpirationDate = DateTime.MaxValue;
      map.Credits = 0;
      map.ExpirationDate = DateTime.MaxValue;

      return map;
    }
  }
}
