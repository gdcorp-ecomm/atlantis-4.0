using System.Collections.Generic;

namespace Atlantis.Framework.BuyerProfileGetById.Interface.BuyerProfileDetails
{
  public class BuyerProfileDetails
  {

    #region properties

    public string ProfileName { get; set; }

    public int RegLength { get; set; }

    public bool AutoRenew { get; set; }

    public bool ParkDNS { get; set; }

    public bool DefaultProfileFlag { get; set; }

    public bool QuickCheckoutFlag { get; set; }

    public List<AddressList> BuyerProfileAddressList { get; set; }

    public List<string> HostNameList { get; set; }

        
    #endregion

    public BuyerProfileDetails(string profileName, int regLength, bool autoRenew, bool parkDNS, 
          bool defaultProfileFlag, bool quickCheckoutFlag, List<AddressList> bpAddressList, List<string> hostNameList)
      {
        ProfileName = profileName;
        RegLength = regLength;
        AutoRenew = autoRenew;
        ParkDNS = parkDNS;
        DefaultProfileFlag = defaultProfileFlag;
        QuickCheckoutFlag = quickCheckoutFlag;
        BuyerProfileAddressList = bpAddressList;
        HostNameList = hostNameList;
      }

  }
}
