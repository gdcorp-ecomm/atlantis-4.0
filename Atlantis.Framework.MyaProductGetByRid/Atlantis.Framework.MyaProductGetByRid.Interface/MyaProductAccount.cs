using System;
using System.Collections.Generic;

using Atlantis.Framework.MyaProduct.Interface;

namespace Atlantis.Framework.MyaProductGetByRid.Interface
{
  public class MyaProductAccount : MyaProductBase
  {
    protected override string AccountExpirationDateKey { get { return "account_expiration_date"; } }
    protected override string BillingResourceIdKey { get { return "resource_id"; } }
    protected override string BundleProductIdKey { get { return "bundle_pf_id"; } }
    protected override string BillingCreditsKey { get { return "billing_credits"; } }
    protected override string CommonNameKey { get { return "commonName"; } }
    protected override string OrionResourceIdKey { get { return "externalResourceID"; } }
    protected override string IsFreeKey { get { return "isFree"; } }
    protected override string IsPastDueKey { get { return "isPastDue"; } }
    protected override string LastExpirationDateKey { get { return "last_expiration_date"; } }
    protected override string NameSpaceKey { get { return "namespace"; } }
    protected override string ObsoleteResourceIdKey { get { return "obsoleteResourceID"; } }
    protected override string ParentBundleIdKey { get { return "parent_bundle_id"; } }
    protected override string ProductIdKey { get { return "pf_id"; } }
    protected override string ProductTypeIdKey { get { return "gdshop_product_typeID"; } }
    protected override string RenewalProductIdKey { get { return "renewal_pf_id"; } }

    private MyaProductType? _productType;
    public override MyaProductType ProductType
    {
      get
      {
        if (!_productType.HasValue)
        {
          if (IsPropertyInDictionary(ProductTypeIdKey))
          {
            _productType = (MyaProductType)Enum.Parse(typeof(MyaProductType), PropertiesDictionary[ProductTypeIdKey].ToString());
          }
        }
        return _productType.Value;
      }
    }

    public MyaProductAccount(int privateLabelId, IDictionary<string, object> propertiesDictionary)
      : base(privateLabelId, propertiesDictionary)
    {
    }

  }
}
