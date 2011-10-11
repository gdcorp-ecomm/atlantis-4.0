
namespace Atlantis.Framework.DCCIsDomainAlertCancellable.Interface
{
  public class UncancellableReasonInfo
  {
    public string DomainName { get; set; }
    public string DomainMonitorId { get; set; }
    public string DomainMonitorBillingId { get; set; }
    public string DomainBackorderStatusId { get; set; }
    public string Error { get; set; }
  }
}
