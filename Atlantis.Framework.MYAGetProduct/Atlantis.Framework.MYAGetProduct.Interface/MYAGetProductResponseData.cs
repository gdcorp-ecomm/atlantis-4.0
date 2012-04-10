using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYAGetProduct.Interface.ProductHelper;
using Atlantis.Framework.MYAGetProduct.Interface.Products;

namespace Atlantis.Framework.MYAGetProduct.Interface
{
  public class MYAGetProductResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private bool _success = false;
    private List<MyaProduct> _myaProducts;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public List<MyaProduct> MyaProducts
    {
      get { return _myaProducts; }
    }

    public MYAGetProductResponseData(List<MyaProduct> myaProducts)
    {
      _myaProducts = myaProducts;
      _success = true;
    }

    public MYAGetProductResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public MYAGetProductResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "MYAGetProductResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sb = new StringBuilder();

      try
      {
        using (XmlWriter writer = XmlWriter.Create(sb))
        {
          writer.WriteStartElement("products");

          foreach (MyaProduct p in MyaProducts)
          {
            writer.WriteStartElement("product");

            switch (p.ProductTypeId)
            {
              case MyaProductTypeId.WebsiteTonight:
                writer.WriteAttributeString("wsDesignPfId", p.DesignPfId.HasValue ? p.DesignPfId.ToString() : null);
                writer.WriteAttributeString("hasMaintenance", p.HasMaintenance.HasValue ? p.HasMaintenance.ToString() : null);
                break;
              case MyaProductTypeId.CustomWebsiteDesign:
                writer.WriteAttributeString("designStatus", p.DesignStatus.HasValue ? p.DesignStatus.ToString() : null);
                writer.WriteAttributeString("customLayoutPfId", p.CustomPageLayoutPfId.HasValue ? p.CustomPageLayoutPfId.ToString() : null);
                writer.WriteAttributeString("serviceFeePfId", p.ServiceFeePfId.HasValue ? p.ServiceFeePfId.ToString() : null);
                writer.WriteAttributeString("rowId", p.RowId.HasValue ? p.RowId.ToString() : null);
                writer.WriteAttributeString("orderId", p.OrderId.ToString());
                break;
              case MyaProductTypeId.ExpressEmailMarketing:
                writer.WriteAttributeString("activeQuota", p.ActiveQuota.HasValue ? p.ActiveQuota.ToString() : null);
                writer.WriteAttributeString("addOnQuota", p.AddOnQuota.HasValue ? p.AddOnQuota.ToString() : null);
                writer.WriteAttributeString("customerId", p.CustomerId.HasValue ? p.CustomerId.ToString() : null);
                writer.WriteAttributeString("entityId", p.EntityId.HasValue ? p.EntityId.ToString() : null);
                writer.WriteAttributeString("parentBundleProductTypeId", p.ParentBundleProductTypeId.HasValue ? p.ParentBundleProductTypeId.ToString() : null);
                writer.WriteAttributeString("pendingQuota", p.PendingQuota.HasValue ? p.PendingQuota.ToString() : null);
                writer.WriteAttributeString("startDate", p.StartDate.HasValue ? p.StartDate.ToString() : null);
                break;
              //case MyaProductTypeId.CustomerManager:
              //case MyaProductTypeId.MarketPlace:
              //  writer.WriteAttributeString("canRenewFreeProducts", p.CanRenewFreeProducts.HasValue ? p.CanRenewFreeProducts.ToString() : null);
              //  break;
              case MyaProductTypeId.TrafficBlazer:
                writer.WriteAttributeString("userWebsiteId", p.UserWebsiteId.HasValue ? p.UserWebsiteId.ToString() : null);
                writer.WriteAttributeString("dateSubmitted", p.DateSubmitted.HasValue ? p.DateSubmitted.ToString() : null);
                break;
              case MyaProductTypeId.Reseller:
                writer.WriteAttributeString("privateLabelGroupType", p.PrivateLabelGroupType.HasValue ? p.PrivateLabelGroupType.ToString() : null);
                writer.WriteAttributeString("entityId", p.EntityId.HasValue ? p.EntityId.ToString() : null);
                writer.WriteAttributeString("parentBundleProductTypeId", p.ParentBundleProductTypeId.HasValue ? p.ParentBundleProductTypeId.ToString() : null);
                break;
              case MyaProductTypeId.MerchantAccount:
                writer.WriteAttributeString("merchantAccountID", p.MerchantAccountId.HasValue ? p.MerchantAccountId.ToString() : null);
                writer.WriteAttributeString("createDate", p.CreateDate.HasValue ? p.CreateDate.ToString() : null);
                writer.WriteAttributeString("authenticationGUID", p.AuthenticationGUID.ToString());
                writer.WriteAttributeString("applicationDescription", p.ApplicationDescription.ToString());
                writer.WriteAttributeString("supportPhone", p.SupportPhone.ToString());
                writer.WriteAttributeString("orderId", p.OrderId.ToString());
                writer.WriteAttributeString("referralId", p.ReferralId.ToString());
                break;
            }
            writer.WriteAttributeString("autoRenewFlag", p.AutoRenewFlag.ToString());
            writer.WriteAttributeString("accountExpirationDate", p.AccountExpirationDate.ToString());
            writer.WriteAttributeString("bundlePfId", p.BundlePfId.HasValue ? p.BundlePfId.ToString() : null);
            writer.WriteAttributeString("commonName", p.CommonName.ToString());
            writer.WriteAttributeString("credits", p.Credits.ToString());
            writer.WriteAttributeString("description", p.Description.ToString());
            writer.WriteAttributeString("expirationDate", p.ExpirationDate.ToString());
            writer.WriteAttributeString("externalResourceId", p.OrionResourceId.ToString());
            writer.WriteAttributeString("isFree", p.IsFree.ToString());
            writer.WriteAttributeString("isPastDue", p.IsPastDue.ToString());
            writer.WriteAttributeString("lastExpirationDate", p.LastExpirationDate.HasValue ? p.LastExpirationDate.ToString() : null);
            writer.WriteAttributeString("namespace", p.Namespace.ToString());
            writer.WriteAttributeString("numberOfPeriods", p.NumberOfPeriods.ToString());
            writer.WriteAttributeString("obsoleteResourceId", p.ObsoleteResourceId.HasValue ? p.ObsoleteResourceId.ToString() : null);
            writer.WriteAttributeString("parentBundleId", p.ParentBundleId.HasValue ? p.ParentBundleId.ToString() : null);
            writer.WriteAttributeString("pfId", p.ProductId.ToString());
            writer.WriteAttributeString("ppShopperProfileId", p.PpShopperProfileId.HasValue ? p.PpShopperProfileId.ToString() : string.Empty);
            writer.WriteAttributeString("productTypeId", p.ProductTypeId.ToString());
            writer.WriteAttributeString("recurringPayment", p.RecurringPayment.ToString());
            writer.WriteAttributeString("renewalPfId", p.RenewalPfId.HasValue ? p.RenewalPfId.ToString() : null);
            writer.WriteAttributeString("resourceId", p.BillingResourceId.ToString());

            writer.WriteEndElement();
          }
          writer.WriteEndElement();
        }
      }
      catch (Exception ex)
      {
        throw new AtlantisException("MyaGetProductResponseData::ToXml", string.Empty, string.Empty, "Error Converting Response Object To XML", ex.Message, string.Empty, string.Empty, string.Empty, string.Empty, 0);
      }

      return sb.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
