
namespace Atlantis.Framework.DCCGetTransfersInProgress.Interface
{
  public class DomainTransfer
  {
    public string DomainName { get; set; }
    public string DomainId { get; set; }
    public string Status { get; set; }
    public string ErrorDescription { get; set; }
    public string IntlDomaiNname { get; set; }
    public int TransferTypeId { get; set; }
    public string TransferType { get; set; }
    public int StepCount { get; set; }
    public int TransferStepId { get; set; }
    public int StepNumber { get; set; }
    public string InternalStepName { get; set; }
    public string ExternalStepName { get; set; }
    public string InternalStepDescription { get; set; }
    public string ExternalStepDescription { get; set; }
    public int TransferStepStatusId { get; set; }
    public string InternalStatusDescription { get; set; }
    public string ExternalStatusDescription { get; set; }
    public string InternalActionToTake { get; set; }
    public string ExternalActionToTake { get; set; }
    public string InternalShortActionToTake { get; set; }
    public string ExternalShortActionToTake { get; set; }
  }
}
