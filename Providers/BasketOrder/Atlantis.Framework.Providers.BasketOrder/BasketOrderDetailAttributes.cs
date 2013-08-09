namespace Atlantis.Framework.Providers.BasketOrder
{
  public static class BasketOrderDetailAttributes
  {
    public const string TotalTotal = "_total_total";
    public const string TotalTotalUsd = "_total_total_usd";
    public const string SubTotal = "_oadjust_subtotal";
    public const string BaseSubTotal = "_base_subtotal";
    public const string ItemCount = "itemcount";
    public const string OrderId = "order_id";
    public const string MerchantAccountID = "merchantaccountid";

    public const string ShippingPrice = "shipping_price";

    public const string ShippingTotal = "_shipping_total";

    public const string HandlingTotal = "_handling_total";
    public const string ThirdPartyAmnt = "third_party_shipping_amount";
    public const string ThirdPartyShippingMethod = "third_party_shipping_method";
    public const string ThirdPartyShippingOptions = "third_party_shipping_options";

    public const string TaxTotal = "_tax_total";
    public const string InstoreCredit = "_instore_credit_amount";
    public const string MembershipLevel = "membershiplevel";

    public const string PrepaidDiscountTotal = "prepaid_discount_total";
    public const string OrderDiscountDescription = "_oadjust_subtotal_discount_description";
    public const string OrderDiscountAmount = "_oadjust_subtotal_discount";
    public const string OrderDiscountCode = "orderdiscountcode";

    public const string BasketType = "basket_type";

    public const string Yard = "yard";
    public const string SourceCode = "source_code";

    public const string CurrencyDisplayed = "currencydisplay";

    public const string OldSpendAmount = "_old_spend";
    public const string OldDiscount = "_old_discount";
    public const string OldAwardType = "_old_award_type";

    public const string TransactionCurrency = "transactioncurrency";
    public const string OperatingCurrency = "operatingcurrency";
    public const string ConversionRate = "conversionrate2o";

    public const string ShipToFirstname = "ship_to_first_name";
    public const string ShipToLastName = "ship_to_last_name";

    public const string ShipToStreet1 = "ship_to_street1";
    public const string ShipToStreet2 = "ship_to_street2";
    public const string ShipToState = "ship_to_state";
    public const string ShipToProvince = "ship_to_province";
    public const string ShipToEmail = "ship_to_email";
    public const string ShipToFax = "ship_to_fax";
    public const string ShipToZip = "ship_to_zip";
    public const string ShipToCountry = "ship_to_country";
    public const string ShipToCompany = "ship_to_company";
    public const string ShipToCity = "ship_to_city";
    public const string ShipToHomePhone = "ship_to_phone2";
    public const string ShipToWorkPhone = "ship_to_phone1";
    public const string ShipToMethod = "shipping_method";

    public const string BillToFirstname = "bill_to_first_name";
    public const string BillToLastName = "bill_to_last_name";
    public const string BillToState = "bill_to_state";
    public const string BillToPhone1 = "bill_to_phone1";
    public const string BillToPhone2 = "bill_to_phone2";
    public const string BillToCompany = "bill_to_company";
    public const string BillToStreet1 = "bill_to_street1";
    public const string BillToStreet2 = "bill_to_street2";
    public const string BillToZip = "bill_to_zip";
    public const string BillToCountry = "bill_to_country";
    public const string BasketTransferLocked = "basket_transfer_locked"; //TODO: Should this stop the transfer of a basket
    public const string BillToEmail = "bill_to_email";
    public const string BillToCity = "bill_to_city";

    public const string TaxIncluded = "_tax_included";
    public const string TaxType = "_tax_type";
    public const string TaxId = "tax_id";

    public const string ShopperID = "shopper_id";
    public const string ShopperSourceCode = "_shopper_source_code";

    public const string BasketPaymentTypeFlag = "basketPaymentTypeFlag";
    public const string RestrictedPaymentTypes = "_restricted_payment_type";

    public const string CustomShopMoreID = "_custom_shopmore_id";

    public const string ConversionRateToUsd = "conversionratet2u";
  }
}
