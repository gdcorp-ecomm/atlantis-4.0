using System;
using System.Data;
using System.Xml;
using Atlantis.Framework.MYAGetProduct.Interface.ProductHelper;

namespace Atlantis.Framework.MYAGetProduct.Interface.Products
{
  public class WebsiteTonightProduct : MyaProduct  //can this be separated into 2 products?  yes
  {

    #region Constructors
    public WebsiteTonightProduct() { }

    public WebsiteTonightProduct(MyaProduct mpl,
      int? customPageLayoutPfId,
      int? designPfId,
      int? designStatus,
      bool? hasMaintenance,
      string orderId,
      int? rowId,
      int? serviceFeePfId)
    {
      if (mpl == null)
        return;

      AccountExpirationDate = mpl.AccountExpirationDate;
      BundlePfId = mpl.BundlePfId;
      CommonName = mpl.CommonName;
      Credits = mpl.Credits;
      Description = mpl.Description;
      ExpirationDate = mpl.ExpirationDate;
      OrionResourceId = mpl.OrionResourceId;
      IsFree = mpl.IsFree;
      IsPastDue = mpl.IsPastDue;
      LastExpirationDate = mpl.LastExpirationDate;
      Namespace = mpl.Namespace;
      ObsoleteResourceId = mpl.ObsoleteResourceId;
      ParentBundleId = mpl.ParentBundleId;
      ProductId = mpl.ProductId;
      ProductTypeId = mpl.ProductTypeId;
      RenewalPfId = mpl.RenewalPfId;
      BillingResourceId = mpl.BillingResourceId;
      CustomPageLayoutPfId = customPageLayoutPfId;
      DesignPfId = designPfId;
      DesignStatus = designStatus;
      HasMaintenance = hasMaintenance;
      OrderId = orderId;
      RowId = rowId;
      ServiceFeePfId = serviceFeePfId;
    }
    #endregion

    #region Methods

    public override MyaProduct PopulateObjectFromDB(IDataReader dr, MYAGetProductRequestData myaProductRequestData, bool hasIsFreeColumn)
    {
      MyaProduct wst = new WebsiteTonightProduct();

      wst = base.PopulateObjectFromDB(dr, myaProductRequestData, hasIsFreeColumn);

      if (myaProductRequestData.ProductTypeId == MyaProductTypeId.CustomWebsiteDesign)
      {
        PopulateObjectFromDbForCWD(dr, myaProductRequestData, wst);
      }
      else
      {
        PopulateObjectFromDbForWST(dr, myaProductRequestData, wst);
      }

      return wst;
    }

    private static void PopulateObjectFromDbForCWD(IDataReader dr, MYAGetProductRequestData myaProductRequestData, MyaProduct wst)
    {
      wst.DesignStatus = dr["gdshop_billing_statusID"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["gdshop_billing_statusID"]);
      wst.CustomPageLayoutPfId = dr["custom_layout_pf_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["custom_layout_pf_id"]);
      wst.ServiceFeePfId = dr["service_fee_pf_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["service_fee_pf_id"]);
      wst.RowId = dr["row_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["row_id"]);
      wst.OrderId = dr["order_id"] == DBNull.Value ? null : Convert.ToString(dr["order_id"]);
    }

    private static void PopulateObjectFromDbForWST(IDataReader dr, MYAGetProductRequestData myaProductRequestData, MyaProduct wst)
    {
      wst.DesignPfId = dr["wsdesign_pf_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["wsdesign_pf_id"]);
      wst.HasMaintenance = dr["hasMaintenance"] == DBNull.Value ? null : (bool?)Convert.ToBoolean(dr["hasMaintenance"]);
    }
    #endregion
  }
}