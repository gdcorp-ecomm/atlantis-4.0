namespace Atlantis.Framework.EcommInvoices.Interface
{
  public static class InvoiceStatus
  {
    public const int All = -1;
    public const int Created = 1;
    public const int Active = 2;
    public const int Cancelled = 3;
    public const int Completed = 4;
    public const int Failed = 5;
    public const int Expired30 = 6; //new status to return for invoices that we expire automatically after approximately 30 days
  }
  public static class InvoiceProcessorStatus
  {
    public const int Approved = 0;
    public const int Declined = 1;
    public const int UserCancel = 2;
    public const int Timeout = 3;
    public const int Error = 4;
    public const int Pending = 5;
    public const int NoResponse = 6;
  }
}
