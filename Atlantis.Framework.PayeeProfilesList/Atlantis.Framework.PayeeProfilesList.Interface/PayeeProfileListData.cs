
namespace Atlantis.Framework.PayeeProfilesList.Interface
{
  public class PayeeProfileListData
  {
    public string CapId{ get; set; }

    public string FriendlyName{ get; set; }


    public bool IsPayable { get; set; }
    public string NonPayableReason { get; set; }

    internal PayeeProfileListData(string capId, string friendlyName, string isPayable, string nonPayableReason)
    {
      CapId = capId;
      FriendlyName = friendlyName;
      IsPayable = string.Equals(isPayable, "1");
      NonPayableReason = nonPayableReason;
    }
  }
}
