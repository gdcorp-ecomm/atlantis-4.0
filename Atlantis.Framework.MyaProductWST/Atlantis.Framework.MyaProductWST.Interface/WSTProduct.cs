using System;
using System.Collections.Generic;
using Atlantis.Framework.MyaProduct.Interface;

namespace Atlantis.Framework.MyaProductWST.Interface
{
  public class WSTProduct : MyaProductBase, IMyaProductAutoRenewable, IMyaProductPaymentProfile
  {
    protected override string AccountExpirationDateKey { get { return "account_expiration_date"; } }

    protected override string BillingResourceIdKey { get { return "resource_id"; } }

    protected override string BundleProductIdKey { get { return "bundle_pf_id"; } }

    protected override string BillingCreditsKey { get { return "billing_credits"; } }

    protected override string CommonNameKey { get { return "commonName"; } }

    protected override string OrionResourceIdKey { get { return "externalResourceID"; } }

    protected override string IsPastDueKey { get { return "isPastDue"; } }

    protected override string IsFreeKey { get { return "isPastDue"; } }

    protected override string LastExpirationDateKey { get { return "last_expiration_date"; } }

    protected override string NameSpaceKey { get { return "namespace"; } }

    protected override string ObsoleteResourceIdKey { get { return "obsoleteResourceID"; } }

    protected override string ParentBundleIdKey { get { return "parent_bundle_id"; } }

    protected override string ProductIdKey { get { return "pf_id"; } }

    protected override string ProductTypeIdKey { get { return "gdshop_product_typeID"; } }

    protected override string RenewalProductIdKey { get { return "renewal_pf_id"; } }

    public override MyaProductType ProductType
    {
      get { return MyaProductType.WebSiteTonight; }
    }

    private bool? _autoRenewFlag;
    public bool? AutoRenewFlag
    {
      get
      {
        if (_autoRenewFlag == null)
        {
          if (IsPropertyInDictionary("autoRenewFlag"))
          {
            _autoRenewFlag = Convert.ToBoolean(PropertiesDictionary["autoRenewFlag"]);
          }
        }
        return _autoRenewFlag;
      }
    }

    private int? _paymentProfileShopperProfileId;
    public int? PaymentProfileShopperProfileId
    {
      get
      {
        if (_paymentProfileShopperProfileId == null)
        {
          _paymentProfileShopperProfileId = 0;

          if (IsPropertyInDictionary("pp_shopperProfileID"))
          {
            _paymentProfileShopperProfileId = Convert.ToInt32(PropertiesDictionary["pp_shopperProfileID"]);
          }
        }
        return _paymentProfileShopperProfileId;
      }
    }

    public override bool IsFree
    {
      get
      {
        return ParentBundleId == null || ParentBundleId.Value == 0;
      }
    }

    private bool? _hasMaintenance;
    public bool HasMaintenance
    {
      get
      {
        if (_hasMaintenance == null)
        {
          _hasMaintenance = false;
          if (IsPropertyInDictionary("hasMaintenance"))
          {
            _hasMaintenance = Convert.ToBoolean(PropertiesDictionary["hasMaintenance"]);
          }
        }
        return _hasMaintenance.Value;
      }
    }

    private int? _designProductId;
    public int? DesignProductId
    {
      get
      {
        if (_designProductId == null)
        {
          if (IsPropertyInDictionary("wsdesign_pf_id"))
          {
            _designProductId = Convert.ToInt32(PropertiesDictionary["wsdesign_pf_id"]);
          }
        }
        return _designProductId;
      }
    }

    public WSTProduct(int privateLabelId, IDictionary<string, object> propertiesDictionary)
      : base(privateLabelId, propertiesDictionary)
    {
    }
  }
}