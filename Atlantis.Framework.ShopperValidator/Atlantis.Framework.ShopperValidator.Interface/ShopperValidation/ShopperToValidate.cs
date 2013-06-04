using System.Collections.Generic;

namespace Atlantis.Framework.ShopperValidator.Interface.ShopperValidation
{
  public class ShopperToValidate
  {
    #region Properties

    #region Public ShopperProperties

    public ShopperProperty FirstName { get; set; }
    public ShopperProperty LastName { get; set; }
    public ShopperProperty Address1 { get; set; }
    public ShopperProperty Address2 { get; set; }
    public ShopperProperty Email { get; set; }
    public ShopperProperty City { get; set; }
    public ShopperProperty State { get; set; }
    public ShopperProperty Zip { get; set; }
    public ShopperProperty Country { get; set; }
    public ShopperProperty PhoneWork { get; set; }
    public ShopperProperty PhoneWorkExtension { get; set; }
    public ShopperProperty PhoneHome { get; set; }
    public ShopperProperty PhoneMobile { get; set; }
    public ShopperProperty PhoneMobileSurvey { get; set; }
    public ShopperProperty PhoneMobileSurveyCarrier { get; set; }
    public ShopperProperty Username { get; set; }
    public ShopperProperty Password { get; set; }
    public ShopperProperty PasswordConfirm { get; set; }
    public ShopperProperty PasswordHint { get; set; }
    public ShopperProperty CallInPin { get; set; }
    public ShopperProperty BirthDay { get; set; }
    public ShopperProperty BirthMonth { get; set; }
    public ShopperProperty AccountUsageType { get; set; }

    #endregion

    private HashSet<ShopperProperty> _allShopperProperties;
    public HashSet<ShopperProperty> AllShopperProperties
    {
      get
      {
        if (_allShopperProperties == null)
        {
          _allShopperProperties = new HashSet<ShopperProperty>();
        }

        return _allShopperProperties;
      }
    }

    #region Private Properties
    private string _requestUrl = string.Empty;
    private string _pathway = string.Empty;
    private int _pageCount = 0;


    #endregion

    #endregion

    #region Shopper constructors

    public ShopperToValidate()
    {
      _allShopperProperties = new HashSet<ShopperProperty>();

      #region Initialize All Shopper Properties And Add To AllShopperProperties container.
      FirstName = CreateShopperProperty();
      LastName = CreateShopperProperty();
      Address1 = CreateShopperProperty();
      Address2 = CreateShopperProperty();
      Email = CreateShopperProperty();
      City = CreateShopperProperty();
      State = CreateShopperProperty();
      Zip = CreateShopperProperty();
      Country = CreateShopperProperty();
      PhoneWork = CreateShopperProperty();
      PhoneWorkExtension = CreateShopperProperty();
      PhoneHome = CreateShopperProperty();
      PhoneMobile = CreateShopperProperty();
      PhoneMobileSurvey = CreateShopperProperty();
      PhoneMobileSurveyCarrier = CreateShopperProperty();
      Username = CreateShopperProperty();
      Password = CreateShopperProperty();
      PasswordConfirm = CreateShopperProperty();
      PasswordHint = CreateShopperProperty();
      CallInPin = CreateShopperProperty();
      BirthDay = CreateShopperProperty();
      BirthMonth = CreateShopperProperty();
      AccountUsageType = CreateShopperProperty();

      #endregion
    }

    public ShopperToValidate(string requestUrl, string pathway, int pageCount)
      : this()
    {
      _requestUrl = requestUrl;
      _pathway = pathway;
      _pageCount = pageCount;
    }

    private ShopperProperty CreateShopperProperty()
    {
      ShopperProperty shopperProperty = new ShopperProperty();
      _allShopperProperties.Add(shopperProperty);

      return shopperProperty;
    }

    #endregion

    #region Shopper required field init methods

    // StandardShopper is the extended account creation requirements before the quick creation initiative
    public void InitStandardRequiredFields()
    {
      // location
      this.Address1.IsRequired = true;
      this.City.IsRequired = true;
      this.Country.IsRequired = true;
      this.State.IsRequired = true;
      this.Zip.IsRequired = true;

      // personal
      this.FirstName.IsRequired = true;
      this.LastName.IsRequired = true;

      // contact
      this.Email.IsRequired = true;
      this.PhoneWork.IsRequired = true;

      // credentials
      this.Username.IsRequired = true;
      this.Password.IsRequired = true;
      this.PasswordHint.IsRequired = true;
      this.CallInPin.IsRequired = true;
    }

    // SlimShopper is the limited account creation requirements after the quick creation initiative
    public void InitSlimRequiredFields()
    {
      // personal
      this.FirstName.IsRequired = true;
      this.LastName.IsRequired = true;

      // contact
      this.Email.IsRequired = true;

      // credentials
      this.Username.IsRequired = true;
      this.Password.IsRequired = true;
    }

    #endregion
  }
}
