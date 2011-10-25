
namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public class PurchasePaymentOrigin : PurchaseElement
  {
    public override string ElementName
    {
      get { return "PaymentOrigin"; }
    }

    public string RepVersion
    {
      get { return GetStringProperty("_repversion", string.Empty); }
      set { this["_repversion"] = value; }
    }

    public string WebServer
    {
      get { return GetStringProperty("_webserver", string.Empty); }
      set { this["_webserver"] = value; }
    }

    public string EnteredBy
    {
      get { return GetStringProperty("entered_by", string.Empty); }
      set { this["entered_by"] = value; }
    }

    public string RefundedBy
    {
      get { return GetStringProperty("refunded_by", string.Empty); }
      set { this["refunded_by"] = value; }
    }

    public string FromApp
    {
      get { return GetStringProperty("from_app", string.Empty); }
      set { this["from_app"] = value; }
    }

    public string OrderBilling
    {
      get { return GetStringProperty("order_billing", string.Empty); }
      set { this["order_billing"] = value; }
    }

    public string OrderSource
    {
      get { return GetStringProperty("order_source", string.Empty); }
      set { this["order_source"] = value; }
    }

    public string RemoteAddress
    {
      get { return GetStringProperty("remote_addr", string.Empty); }
      set { this["remote_addr"] = value; }
    }

    public string RemoteHost
    {
      get { return GetStringProperty("remote_host", string.Empty); }
      set { this["remote_host"] = value; }
    }

    public string WebSite
    {
      get { return GetStringProperty("_website", string.Empty); }
      set { this["_website"] = value; }
    }

    public string CurrencyDisplay
    {
      get { return GetStringProperty("currencyDisplay", string.Empty); }
      set { this["currencyDisplay"] = value; }
    }
  }
}
