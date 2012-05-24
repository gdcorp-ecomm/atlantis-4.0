using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.BizRegDomain.Interface
{
  public class LocalBusiness
  {
    // a replacement for the Business class and it's child classes

    public enum RegistrationMode
    {
      BizReg,
      Free
    }

    public enum BizStatusCode
    {
      PendingSetup = 1,
      Active = 2,
      Suspended = 4,
      Flagged = 8,
      PendingUpgrade = 16,
      Cancelled = 32
    }

    private int businessIDValue = 0;
    private string gdShopperIDValue = string.Empty;
    private string businessNameValue = string.Empty;
    private string businessDescriptionValue = string.Empty;
    private int businessRegistrationTypeCodeValue = 1;
    private string businessRegistrationNameValue = string.Empty;
    private int businessRatingTypeIDValue = 0;
    private string businessRatingNameValue = string.Empty;
    private bool isReviewEnabledValue = true;
    private bool isFlagEnabledValue = true;
    private bool isRatingEnabledValue = true;
    private int reviewCountValue = 0;
    private int favoriteCountValue = 0;
    private int flagCountValue = 0;
    private int viewCountValue = 0;
    private int businessStatusCodeValue = 1;
    private string businessStatusValue = string.Empty;
    private bool isFlaggedByUserIDValue = false;
    private int businessFlagIDValue = 0;
    private int businessStatusByUserIDValue = 1;
    private int modifyByUserIDValue = 0;
    private bool isUpgradeValue = false;
    private RegistrationMode modeValue = RegistrationMode.BizReg;
    private int marketplaceShopIDValue = 0;
    private int marketplaceStatusMaskValue = 0;
    private bool isNotPublicValue = false;
    private bool isPersonalValue = false;

    // declare child members
    private List<string> categoriesValue = new List<string>();
    private List<int> tagsValue = new List<int>();
    private BusinessFlag businessFlagValue;
    private BusinessLocation businessLocationValue;
    private BusinessRating businessRatingValue;
    private List<BusinessData> bizDataValue = new List<BusinessData>();
    private BusinessDomain businessDomainValue;


    public int BusinessID
    {
      get { return businessIDValue; }
      set { businessIDValue = value; }
    }

    public string GDShopperID
    {
      get { return gdShopperIDValue; }
      set { gdShopperIDValue = value; }
    }

    public string BusinessName
    {
      get { return businessNameValue; }
      set { businessNameValue = value; }
    }

    public string BusinessDescription
    {
      get { return businessDescriptionValue; }
      set { businessDescriptionValue = value; }
    }

    public int BusinessRegistrationTypeCode
    {
      get { return businessRegistrationTypeCodeValue; }
      set { businessRegistrationTypeCodeValue = value; }
    }

    public string BusinessRegistrationName
    {
      get { return businessRegistrationNameValue; }
      set { businessRegistrationNameValue = value; }
    }

    public int BusinessRatingTypeID
    {
      get { return businessRatingTypeIDValue; }
      set { businessRatingTypeIDValue = value; }
    }

    public string BusinessRatingName
    {
      get { return businessRatingNameValue; }
      set { businessRatingNameValue = value; }
    }

    public bool IsReviewEnabled
    {
      get { return isReviewEnabledValue; }
      set { isReviewEnabledValue = value; }
    }

    public bool IsFlagEnabled
    {
      get { return isFlagEnabledValue; }
      set { isFlagEnabledValue = value; }
    }

    public bool IsRatingEnabled
    {
      get { return isRatingEnabledValue; }
      set { isRatingEnabledValue = value; }
    }

    public int ReviewCount
    {
      get { return reviewCountValue; }
      set { reviewCountValue = value; }
    }

    public int FavoriteCount
    {
      get { return favoriteCountValue; }
      set { favoriteCountValue = value; }
    }

    public int FlagCount
    {
      get { return flagCountValue; }
      set { flagCountValue = value; }

    }

    public int ViewCount
    {
      get { return viewCountValue; }
      set { viewCountValue = value; }
    }

    public int BusinessStatusCode
    {
      get { return businessStatusCodeValue; }
      set { businessStatusCodeValue = value; }
    }

    public string BusinessStatus
    {
      get { return businessStatusValue; }
      set { businessStatusValue = value; }
    }

    public bool IsFlaggedByUserID
    {
      get { return isFlaggedByUserIDValue; }
      set { isFlaggedByUserIDValue = value; }
    }

    public int BusinessFlagID
    {
      get { return businessFlagIDValue; }
      set { businessFlagIDValue = value; }
    }

    public int BusinessStatusByUserID
    {
      get { return businessStatusByUserIDValue; }
      set { businessStatusByUserIDValue = value; }
    }

    public int ModifyByUserID
    {
      get { return modifyByUserIDValue; }
      set { modifyByUserIDValue = value; }
    }

    public bool IsUpgrade
    {
      get { return isUpgradeValue; }
      set { isUpgradeValue = value; }
    }

    public string Mode
    {
      get { return modeValue.ToString(); }
      set { modeValue = (RegistrationMode)Enum.Parse(typeof(RegistrationMode), value, true); }
    }

    public int MarketplaceShopID
    {
      get { return marketplaceShopIDValue; }
      set { marketplaceShopIDValue = value; }
    }

    public int MarketplaceStatusMask
    {
      get { return marketplaceStatusMaskValue; }
      set { marketplaceStatusMaskValue = value; }
    }

    public bool IsNotPublic
    {
      get { return isNotPublicValue; }
      set { isNotPublicValue = value; }
    }

    public bool IsPersonal
    {
      get { return isPersonalValue; }
      set { isPersonalValue = value; }
    }


    public List<string> CategoryPaths
    {
      get { return categoriesValue; }
      set { categoriesValue = value; }
    }

    public List<int> Tags
    {
      get { return tagsValue; }
      set { tagsValue = value; }

    }

    public BusinessFlag Flag
    {
      get { return businessFlagValue; }
      set { businessFlagValue = value; }
    }

    public BusinessLocation Location
    {
      get { return businessLocationValue; }
      set { businessLocationValue = value; }
    }

    public BusinessRating Rating
    {
      get { return businessRatingValue; }
      set { businessRatingValue = value; }
    }

    public BusinessDomain Domain
    {
      get { return businessDomainValue; }
      set { businessDomainValue = value; }
    }

    public List<BusinessData> BizData
    {
      get { return bizDataValue; }
      set { bizDataValue = value; }
    }
  }


  public class BusinessCategory
  {
    private int businessCategoryIDValue = 0;
    private int businessIDValue = 0;
    private int categoryIDValue = -1;
    private int parentIDValue = 0;
    private int rootIDValue = 0;
    private string titleValue = string.Empty;
    private string pathValue = string.Empty;
    private int modifyByUserIDValue = 1;
    private int businessCountValue = 0;
    private int viewCountValue = 0;
    private string rewriteURLValue = string.Empty;
    private DateTime createDateValue = DateTime.Today;
    private int createByUserIDValue = 1;


    public int BusinessCategoryID
    {
      get { return businessCategoryIDValue; }
      set { businessCategoryIDValue = value; }
    }

    public int BusinessID
    {
      get { return businessIDValue; }
      set { businessIDValue = value; }
    }

    public int CategoryID
    {
      get { return categoryIDValue; }
      set { categoryIDValue = value; }
    }

    public int ParentID
    {
      get { return parentIDValue; }
      set { parentIDValue = value; }
    }

    public int RootID
    {
      get { return rootIDValue; }
      set { rootIDValue = value; }
    }

    public string Title
    {
      get { return titleValue; }
      set { titleValue = value; }
    }

    public string Path
    {
      get { return pathValue; }
      set { pathValue = value; }
    }

    public int ModifyByUserID
    {
      get { return modifyByUserIDValue; }
      set { modifyByUserIDValue = value; }
    }

    public int BusinessCount
    {
      get { return businessCountValue; }
      set { businessCountValue = value; }
    }

    public int ViewCount
    {
      get { return viewCountValue; }
      set { viewCountValue = value; }
    }

    public string RewriteURL
    {
      get { return rewriteURLValue; }
      set { rewriteURLValue = value; }
    }

    public DateTime CreateDate
    {
      get { return createDateValue; }
      set { createDateValue = value; }
    }

    public int CreateByUserID
    {
      get { return createByUserIDValue; }
      set { createByUserIDValue = value; }
    }
  }

  public class BusinessData
  {
    private int businessDataIDValue = 0;
    private int businessIDValue = 0;
    private string dataTypeValue = string.Empty;
    private string displayTextValue = string.Empty;
    private string urlValue = string.Empty;
    private string altTextValue = string.Empty;
    private int positionValue = 0;
    private int modifyByUserIDValue = 1;
    private bool isImageAvailableValue = false;
    private string imageFileValue = string.Empty;
    private string imagePathValue = string.Empty;
    private string imageUrlValue = string.Empty;
    private int imageWidthValue = 0;
    private int imageHeightValue = 0;

    public int BusinessDataID
    {
      get { return businessDataIDValue; }
      set { businessDataIDValue = value; }
    }

    public int BusinessID
    {
      get { return businessIDValue; }
      set { businessIDValue = value; }
    }

    public string DataType
    {
      get { return dataTypeValue; }
      set { dataTypeValue = value; }
    }

    public string DisplayText
    {
      get { return displayTextValue; }
      set { displayTextValue = value; }
    }

    public string URL
    {
      get { return urlValue; }
      set { urlValue = value; }
    }

    public string AltText
    {
      get { return altTextValue; }
      set { altTextValue = value; }
    }

    public int Position
    {
      get { return positionValue; }
      set { positionValue = value; }
    }

    public int ModifyByUserID
    {
      get { return modifyByUserIDValue; }
      set { modifyByUserIDValue = value; }
    }

    public bool IsImageAvailable
    {
      get { return isImageAvailableValue; }
      set { isImageAvailableValue = value; }
    }

    public int ImageHeight
    {
      get { return imageHeightValue; }
      set { imageHeightValue = value; }
    }

    public int ImageWidth
    {
      get { return imageWidthValue; }
      set { imageWidthValue = value; }
    }

    public string ImagePath
    {
      get { return imagePathValue; }
      set { imagePathValue = value; }
    }

    public string ImageUrl
    {
      get { return imageUrlValue; }
      set { imageUrlValue = value; }
    }

    public string ImageFile
    {
      get { return imageFileValue; }
      set { imageFileValue = value; }
    }

  }


  public class BusinessFlag
  {
    private int businessFlagIDValue = 0;
    private int userIDValue = -1;
    private int businessIDValue = 0;
    private string userScreenNameValue = string.Empty;
    private bool isUserAvatarAvailableValue = false;
    private int flagTypeIDValue = 0;
    private string flagTypeValue = string.Empty;
    private string flagReasonValue = string.Empty;
    private bool isClearedValue = false;
    private int moderatedByUserIDValue = -1;
    private string moderatedByUserScreenNameValue = string.Empty;
    private string moderatedByManagerUserNameValue = string.Empty;
    private bool isModeratedByUserAvatarAvailableValue = false;
    private DateTime moderatedDateValue = DateTime.Now;
    private DateTime createDateValue = DateTime.Now;
    private int modifyByUserIDValue = -1;
    private DateTime modifyDateValue = DateTime.Now;

    public int BusinessFlagID
    {
      get
      { return businessFlagIDValue; }

      set
      {
        if (!businessFlagIDValue.Equals(value))
          businessFlagIDValue = value;
      }
    }

    public int UserID
    {
      get
      { return userIDValue; }

      set
      {
        if (!userIDValue.Equals(value))
          userIDValue = value;
      }
    }

    public int BusinessID
    {
      get
      { return businessIDValue; }

      set
      {
        if (!businessIDValue.Equals(value))
          businessIDValue = value;
      }
    }

    public string UserScreenName
    {
      get
      { return userScreenNameValue; }

      set
      {
        if (value == null) value = string.Empty;
        if (!userScreenNameValue.Equals(value))
          userScreenNameValue = value;
      }
    }

    public bool IsUserAvatarAvailable
    {
      get
      { return isUserAvatarAvailableValue; }

      set
      {
        if (!isUserAvatarAvailableValue.Equals(value))
          isUserAvatarAvailableValue = value;
      }
    }

    public int FlagTypeID
    {
      get
      { return flagTypeIDValue; }

      set
      {
        if (!flagTypeIDValue.Equals(value))
          flagTypeIDValue = value;
      }
    }

    public string FlagType
    {
      get
      { return flagTypeValue; }

      set
      {
        if (value == null) value = string.Empty;
        if (!flagTypeValue.Equals(value))
          flagTypeValue = value;
      }
    }

    public string FlagReason
    {
      get
      { return flagReasonValue; }

      set
      {
        if (value == null) value = string.Empty;
        if (!flagReasonValue.Equals(value))
          flagReasonValue = value;
      }
    }

    public bool IsCleared
    {
      get
      { return isClearedValue; }

      set
      {
        if (!isClearedValue.Equals(value))
          isClearedValue = value;
      }
    }

    public int ModeratedByUserID
    {
      get
      { return moderatedByUserIDValue; }

      set
      {
        if (!moderatedByUserIDValue.Equals(value))
          moderatedByUserIDValue = value;
      }
    }

    public string ModeratedByUserScreenName
    {
      get
      { return moderatedByUserScreenNameValue; }

      set
      {
        if (value == null) value = string.Empty;
        if (!moderatedByUserScreenNameValue.Equals(value))
          moderatedByUserScreenNameValue = value;
      }
    }

    public string ModeratedByManagerUserName
    {
      get
      { return moderatedByManagerUserNameValue; }

      set
      {
        if (value == null) value = string.Empty;
        if (!moderatedByManagerUserNameValue.Equals(value))
          moderatedByManagerUserNameValue = value;
      }
    }

    public bool IsModeratedByUserAvatarAvailable
    {
      get
      { return isModeratedByUserAvatarAvailableValue; }

      set
      {
        if (!isModeratedByUserAvatarAvailableValue.Equals(value))
          isModeratedByUserAvatarAvailableValue = value;
      }
    }

    public DateTime ModeratedDate
    {
      get
      { return moderatedDateValue.Date; }
    }

    public string ModeratedDateString
    {
      get
      { return moderatedDateValue.ToString("MM/DD/YYYY hh:mm:ss tt"); }

      set
      {
        if (value == null) value = string.Empty;
        moderatedDateValue = DateTime.Parse(value);
      }
    }

    public DateTime CreateDate
    {
      get
      { return createDateValue.Date; }
    }

    public string CreateDateString
    {
      get
      { return createDateValue.ToString("MM/dd/yyyy hh:mm:ss tt"); }

      set
      {
        if (value == null) value = string.Empty;
        createDateValue = DateTime.Parse(value);
      }
    }

    public int ModifyByUserID
    {
      get
      { return modifyByUserIDValue; }

      set
      {
        if (!modifyByUserIDValue.Equals(value))
          modifyByUserIDValue = value;
      }
    }

    public DateTime ModifyDate
    {
      get
      { return modifyDateValue.Date; }
    }

    public string ModifyDateString
    {
      get
      { return modifyDateValue.ToString("MM/DD/YYYY"); }
      set
      {
        if (value == null) value = string.Empty;
        modifyDateValue = DateTime.Parse(value);
      }
    }
  }

  public class BusinessLocation
  {
    private int businessLocationIDValue = 0;
    private int businessIDValue = 0;
    private bool isHQValue = true;
    private string addressLine1Value = string.Empty;
    private string addressLine2Value = string.Empty;
    private string cityValue = string.Empty;
    private string stateValue = string.Empty;
    private string countryValue = string.Empty;
    private string countryCodeValue = string.Empty;
    private string postalCodeValue = string.Empty;
    private string phoneNumber1Value = string.Empty;
    private string phone1CallingCodeValue = string.Empty;
    private string phoneNumber2Value = string.Empty;
    private string phone2CallingCodeValue = string.Empty;
    private string faxNumberValue = string.Empty;
    private string faxCallingCodeValue = string.Empty;
    private string emailValue = string.Empty;
    private string webSiteURLValue = string.Empty;
    private double latitudeValue = 0;
    private double longitudeValue = 0;
    private string businessHoursValue = string.Empty;
    private bool isShowMapValue = true;
    private int modifyByUserIDValue = 1;

    public int BusinessLocationID
    {
      get { return businessLocationIDValue; }
      set { businessLocationIDValue = value; }
    }

    public int BusinessID
    {
      get { return businessIDValue; }

      set
      {
        if (!businessIDValue.Equals(value))
          businessIDValue = value;
      }
    }

    public bool IsHQ
    {
      get
      { return isHQValue; }

      set
      {
        if (!isHQValue.Equals(value))
          isHQValue = value;
      }
    }

    public string AddressLine1
    {
      get
      { return addressLine1Value; }

      set
      {
        if (value == null) value = string.Empty;
        if (!addressLine1Value.Equals(value))
          addressLine1Value = value;
      }
    }

    public string AddressLine2
    {
      get
      { return addressLine2Value; }

      set
      {
        if (value == null) value = string.Empty;
        if (!addressLine2Value.Equals(value))
          addressLine2Value = value;
      }
    }

    public string City
    {
      get
      { return cityValue; }
      set
      {
        if (value == null) value = string.Empty;
        if (!cityValue.Equals(value))
          cityValue = value;
      }
    }

    public string State
    {
      get
      { return stateValue; }
      set
      {
        if (value == null) value = string.Empty;
        if (!stateValue.Equals(value))
          stateValue = value;
      }
    }

    public string Country
    {
      get
      { return countryValue; }

      set
      {
        if (value == null) value = string.Empty;
        if (!countryValue.Equals(value))
          countryValue = value;
      }
    }

    public string CountryCode
    {
      get
      { return countryCodeValue; }

      set
      {
        if (value == null) value = string.Empty;
        if (!countryCodeValue.Equals(value))
          countryCodeValue = value;
      }
    }

    public string PostalCode
    {
      get
      { return postalCodeValue; }

      set
      {
        if (value == null) value = string.Empty;
        if (!postalCodeValue.Equals(value))
          postalCodeValue = value;
      }
    }

    public string PhoneNumber1
    {
      get
      { return phoneNumber1Value.Trim(); }
      set
      {
        if (value == null) value = string.Empty;
        if (!phoneNumber1Value.Equals(value.Trim()))
          phoneNumber1Value = value.Trim();
      }
    }

    public string Phone1CallingCode
    {
      get
      { return phone1CallingCodeValue.Trim(); }
      set
      {
        if (value == null) value = string.Empty;
        if (!phone1CallingCodeValue.Equals(value.Trim()))
          phone1CallingCodeValue = value.Trim();
      }
    }

    public string PhoneNumber2
    {
      get { return phoneNumber2Value.Trim(); }
      set
      {
        if (value == null) value = string.Empty;
        if (!phoneNumber2Value.Equals(value.Trim()))
          phoneNumber2Value = value.Trim();
      }
    }

    public string Phone2CallingCode
    {
      get
      { return phone2CallingCodeValue.Trim(); }
      set
      {
        if (value == null) value = string.Empty;
        if (!phone2CallingCodeValue.Equals(value.Trim()))
          phone2CallingCodeValue = value.Trim();
      }
    }

    public string FaxNumber
    {
      get
      { return faxNumberValue.Trim(); }
      set
      {
        if (value == null) value = string.Empty;
        if (!faxNumberValue.Equals(value.Trim()))
          faxNumberValue = value.Trim();
      }
    }

    public string FaxCallingCode
    {
      get
      { return faxCallingCodeValue.Trim(); }
      set
      {
        if (value == null) value = string.Empty;
        if (!faxCallingCodeValue.Equals(value.Trim()))
          faxCallingCodeValue = value.Trim();
      }
    }

    public string Email
    {
      get
      { return emailValue; }
      set
      {
        if (value == null) value = string.Empty;
        if (!emailValue.Equals(value))
          emailValue = value;
      }
    }

    public string WebSiteURL
    {
      get
      { return webSiteURLValue; }
      set
      {
        if (value == null) value = string.Empty;
        if (!webSiteURLValue.Equals(value))
          webSiteURLValue = value;
      }
    }

    public double Latitude
    {
      get
      { return latitudeValue; }
      set
      {
        if (!latitudeValue.Equals(value))
          latitudeValue = value;
      }
    }

    public double Longitude
    {
      get
      { return longitudeValue; }
      set
      {
        if (!longitudeValue.Equals(value))
          longitudeValue = value;
      }
    }

    public string BusinessHours
    {
      get
      { return businessHoursValue; }
      set
      {
        if (value == null) value = string.Empty;
        if (!businessHoursValue.Equals(value))
          businessHoursValue = value;
      }
    }

    public bool IsShowMap
    {
      get
      { return isShowMapValue; }
      set
      {
        if (!isShowMapValue.Equals(value))
          isShowMapValue = value;
      }
    }

    public int ModifyByUserID
    {
      get { return modifyByUserIDValue; }

      set
      {
        if (!modifyByUserIDValue.Equals(value))
          modifyByUserIDValue = value;
      }
    }
  }

  public class BusinessRating
  {
    private int businessRatingIDValue = 0;
    private int businessIDValue = 0;
    private int businessRatingTypeIDValue = 1;
    private string businessRatingTypeValue = string.Empty;
    private int ratingVotesValue = 0;
    private int totalRatingValue = 0;

    public int BusinessRatingID
    {
      get { return businessRatingIDValue; }
      set { businessRatingIDValue = value; }
    }

    public int BusinessID
    {
      get { return businessIDValue; }
      set
      {
        if (!businessIDValue.Equals(value))
          businessIDValue = value;
      }
    }

    public int BusinessRatingTypeID
    {
      get
      { return businessRatingTypeIDValue; }
      set
      {
        if (!businessRatingTypeIDValue.Equals(value))
          businessRatingTypeIDValue = value;
      }
    }

    public string BusinessRatingType
    {
      get
      { return businessRatingTypeValue; }
      set
      {
        if (value == null) value = string.Empty;
        if (!businessRatingTypeValue.Equals(value))
          businessRatingTypeValue = value;
      }
    }

    public int RatingVotes
    {
      get
      { return ratingVotesValue; }
      set
      {
        if (!ratingVotesValue.Equals(value))
          ratingVotesValue = value;
      }
    }

    public int TotalRating
    {
      get
      { return totalRatingValue; }
      set
      {
        if (!totalRatingValue.Equals(value))
          totalRatingValue = value;
      }
    }
  }

  public class BusinessDomain
  {
    int businessDomainIDValue = 0;
    int businessIDValue = 0;
    int domainIDValue = 0;
    int privateLabelIDValue = 1;
    string domainNameValue = string.Empty;
    string topLevelDomainValue = string.Empty;
    string secondLevelDomainValue = string.Empty;
    private DateTime createDateValue = new DateTime();

    public int BusinessDomainID
    {
      get
      { return businessDomainIDValue; }
      set
      {
        if (!businessDomainIDValue.Equals(value))
          businessDomainIDValue = value;
      }
    }

    public int BusinessID
    {
      get
      { return businessIDValue; }
      set
      {
        if (!businessIDValue.Equals(value))
          businessIDValue = value;
      }
    }

    public int DomainID
    {
      get
      { return domainIDValue; }
      set
      {
        if (!domainIDValue.Equals(value))
          domainIDValue = value;
      }
    }

    public int PrivateLabelID
    {
      get
      { return privateLabelIDValue; }
      set
      {
        if (!privateLabelIDValue.Equals(value))
          privateLabelIDValue = value;
      }
    }

    public string DomainName
    {
      get
      { return domainNameValue; }
      set
      {
        if (value == null) value = string.Empty;
        if (domainNameValue != value)
          domainNameValue = value;
      }
    }

    public string TopLevelDomain
    {
      get
      { return topLevelDomainValue; }
      set
      {
        if (value == null) value = string.Empty;
        if (topLevelDomainValue != value)
          topLevelDomainValue = value;
      }
    }

    public string SecondLevelDomain
    {
      get
      { return secondLevelDomainValue; }
      set
      {
        if (value == null) value = string.Empty;
        if (secondLevelDomainValue != value)
          secondLevelDomainValue = value;
      }
    }

    public DateTime CreateDate
    {
      get
      { return createDateValue.Date; }
    }

    public string CreateDateString
    {
      get
      { return createDateValue.ToString("MM/dd/yyyy hh:mm:ss tt"); }
      set
      {
        if (!createDateValue.Equals(value) && !string.IsNullOrEmpty(value))
          createDateValue = DateTime.Parse(value);
      }
    }
  }
}
