namespace Atlantis.Framework.AccountExecContactInfo.Interface
{
  public enum PortfolioTypes
  {
    PremierServicesPlatinum = 1,
    ResellerAdvisor = 2,
    AMT = 3,
    CAE = 4,
    CN = 5,
    PremierServicesGold = 6,
    PremierServicesSilver = 7    
  }

  public class VipInfo
  {
    #region Properties
    private readonly string _repName;
    public string RepName
    {
      get { return _repName; }
    }

    private readonly string _repEmail;
    public string RepEmail
    {
      get { return _repEmail; }
    }

    private readonly PortfolioTypes _repPortfolioTypeId;
    public PortfolioTypes RepPortfolioTypeId
    {
      get { return _repPortfolioTypeId; }
    }

    private readonly string _repPortfolioType;
    public string RepPortfolioType
    {
      get { return _repPortfolioType; }
    }

    private readonly string _repExternalContactPhone;
    public string RepExternalContactPhone
    {
      get { return _repExternalContactPhone; }
    }
  
    private readonly string _repPhoneExtension;
    public string RepPhoneExtension
    {
      get { return _repPhoneExtension; }
    }
    #endregion

    public VipInfo(string repName, string repEmail, PortfolioTypes typeId, string type, string phone, string ext)
    {
      _repName = repName;
      _repEmail = repEmail;
      _repPortfolioTypeId = typeId;
      _repPortfolioType = type;
      _repExternalContactPhone = phone;
      _repPhoneExtension = ext;
    }
  }
}
