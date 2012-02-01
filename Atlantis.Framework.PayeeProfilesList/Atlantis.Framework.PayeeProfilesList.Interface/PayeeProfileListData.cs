
namespace Atlantis.Framework.PayeeProfilesList.Interface
{
  public class PayeeProfileListData
  {
    private readonly string _capId;
    public string CapId
    {
      get { return _capId; }
    }
    private readonly string _friendlyName;
    public string FriendlyName
    {
      get { return _friendlyName; }
    }

    internal PayeeProfileListData(string capId, string friendlyName)
    {
      _capId = capId;
      _friendlyName = friendlyName;
    }
  }
}
