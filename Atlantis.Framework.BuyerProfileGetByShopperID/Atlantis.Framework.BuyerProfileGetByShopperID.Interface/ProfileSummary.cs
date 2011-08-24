
namespace Atlantis.Framework.BuyerProfileGetByShopperID.Interface
{
  public class ProfileSummary
  {
    private bool _isDefault = false;
    private string _profileId = string.Empty;
    private string _profileName = string.Empty;

    public bool IsDefault
    {
      get
      {
        return _isDefault;
      }
    }

    public string ProfileId
    {
      get
      {
        return _profileId;
      }
    }

    public string ProfileName
    {
      get
      {
        return _profileName;
      }
    }

    public ProfileSummary(string profileId, string profileName, bool isDefault)
    {
      _profileId = profileId;
      _profileName = profileName;
      _isDefault = isDefault;
    }

  }
}
