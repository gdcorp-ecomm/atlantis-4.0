using System;
using System.Data;
using Atlantis.Framework.Providers.DomainLookup.Interface;

namespace Atlantis.Framework.DomainLookup.Interface
{
  public class DomainLookupData : IDomainLookupData
  {
    private DomainLookupData()
    {
    }

    internal DomainLookupData(DataSet domaindata)
    {
      ParseDomainLookupResponse(domaindata);
    }

    public static IDomainLookupData DefaultInstance
    {
      get { return new DomainLookupData(); }
    }

    private int _privateLabelID = 1;
    public int PrivateLabelId { get { return _privateLabelID; } }

    private string _shopperid = string.Empty;
    public string Shopperid { get { return _shopperid; } }

    private bool _isSmartDomain = false;
    public bool IsSmartDomain { get { return _isSmartDomain; } }

    private bool _isMobilized = false;
    public bool IsMobilized { get { return _isMobilized; } }

    private bool _isActive = false;
    public bool IsActive { get { return _isActive; } }

    private DateTime _expirationDate = DateTime.MinValue;
    public DateTime ExpirationDate { get { return _expirationDate; } }

    private int _domainId = 0;
    public int DomainId { get { return _domainId; } }

    private int _tldid = 0;
    public int TldId { get { return _tldid; } }

    private int _isProxied = 0;
    public int IsProxied { get { return _isProxied; } }

    private int _status = 0;
    public int Status { get { return _status; } }

    private bool _isPremiumDomain = false;
    public bool IsPremiumDomain { get { return _isPremiumDomain; } }

    private int _premiumVendorMinPrice = 0;
    public int PremiumVendorMinPrice { get { return _premiumVendorMinPrice; } }

    private int _premiumVendorMaxPrice = 0;
    public int PremiumVendorMaxPrice { get { return _premiumVendorMaxPrice; } }

    private int _premiumVendorRecPrice = 0;
    public int PremiumVendorRecPrice { get { return _premiumVendorRecPrice; } }

    private int _premiumUserListPrice = 0;
    public int PremiumUserListPrice { get { return _premiumUserListPrice; } }

    private bool _isAuction = false;
    public bool IsAuction { get { return _isAuction; } }

    private decimal _auctionPrice = 0;
    public decimal AuctionPrice { get { return _auctionPrice; } }

    private int _auctionTypeId = 0;
    public int AuctionTypeId { get { return _auctionTypeId; } }

    private DateTime _auctionEndTime = DateTime.MinValue;
    public DateTime AuctionEndTime { get { return _auctionEndTime; } }

    private DateTime _xfrAwayDate = DateTime.MinValue;
    public DateTime XfrAwayDate { get { return _xfrAwayDate; } }

    private int _xfrAwayDateUpdateReason = 0;
    public int XfrAwayDateUpdateReason { get { return _xfrAwayDateUpdateReason; } }

    private bool _is60daylock = false;
    public bool Is60DayLock { get { return _is60daylock; } }

    private DateTime _createDate = DateTime.MinValue;
    public DateTime CreateDate { get { return _createDate; } }

    private bool _hasActiveSuspectTerms = false;
    public bool HasSuspectTerms { get { return _hasActiveSuspectTerms; } }

    private int _pdDomainId = -1;
    public int PdDomainId { get { return _pdDomainId; } }

    private string _domainName = "";
    public string DomainName { get { return _domainName; } }

    private int _clientId = -1;
    public int ClientId { get { return _clientId; } }

    private string _client = "";
    public string Client { get { return _client; } }

    private string _adultClient = "";
    public string AdultClient { get { return _adultClient; } }

    private int _channel = -1;
    public int Channel { get { return _channel; } }

    private int _pkid = -1;
    public int Pkid { get { return _pkid; } }

    private string _pkidTemplateId = "";
    public string PkidTemplateId { get { return _pkidTemplateId; } }

    private int _adultStatusMap = -1;
    public int AdultStatusMap { get { return _adultStatusMap; } }

    private string _templateId = "";
    public string TemplateId { get { return _templateId; } }

    private int _subCategoryTypeId = -1;
    public int SubCategoryTypeId { get { return _subCategoryTypeId; } }

    private string _patKeywords = "";
    public string PatKeywords { get { return _patKeywords; } }

    private int _invalidWhoIsStatus = -1;
    public int InvalidWhoIsStatus { get { return _invalidWhoIsStatus; } }

    private int _hasClicks = -1;
    public int HasClicks { get { return _hasClicks; } }

    private int _hasProdRev = -1;
    public int HasProdRev { get { return _hasProdRev; } }

    private DateTime _recordUpdated = DateTime.MinValue;
    public DateTime RecordUpdated { get { return _recordUpdated; } }

    private string _cashParkingTemplateId = "";
    public string CashParkingTemplateId { get { return _cashParkingTemplateId; } }

    private bool _cashParkingActiveFlag = false;
    public bool CashParkingActiveFlag { get { return _cashParkingActiveFlag; } }

    private int _cashParkingAdultMode = -1;
    public int CashParkingAdultMode { get { return _cashParkingAdultMode; } }

    private string _cashParkingKeywords = "";
    public string CashParkingKeywords { get { return _cashParkingKeywords; } }

    private int _cashParkingThemeId = -1;
    public int CashParkingThemeId { get { return _cashParkingThemeId; } }

    private int _cashParkingTemplateTypeCode = -1;
    public int CashParkingTemplateTypeCode { get { return _cashParkingTemplateTypeCode; } }

    private int _cashParkingSubCategoryTypeId = -1;
    public int CashParkingSubCategoryTypeId { get { return _cashParkingSubCategoryTypeId; } }

    private int _cashParkingTrpListingStatusCode = -1;
    public int CashParkingTrpListingStatusCode { get { return _cashParkingTrpListingStatusCode; } }

    private int _cashParkingClientId = -1;
    public int CashParkingClientId { get { return _cashParkingClientId; } }

    private string _cashParkingClient = "";
    public string CashParkingClient { get { return _cashParkingClient; } }

    private string _cashParkingAdultClient = "";
    public string CashParkingAdultClient { get { return _cashParkingAdultClient; } }

    private int _cashParkingChannel = -1;
    public int CashParkingChannel { get { return _cashParkingChannel; } }

    private bool _cashParkingPopundersEnabled = false;
    public bool CashParkingPopundersEnabled { get { return _cashParkingPopundersEnabled; } }

    private bool _cashParkingZeroclickEnabled = false;
    public bool CashParkingZeroclickEnabled { get { return _cashParkingZeroclickEnabled; } }

    private int _cashParkingInquireLinkEnabled = -1;
    public int CashParkingInquireLinkEnabled { get { return _cashParkingInquireLinkEnabled; } }

    private string _customListingText = "";
    public string CustomListingText { get { return _customListingText; } }

    private string _customListingLink = "";
    public string CustomListingLink { get { return _customListingLink; } }

    private void ParseDomainLookupResponse(DataSet domaindata)
    {
      if (domaindata != null)
      {
        if (domaindata.Tables.Count > 0 && domaindata.Tables[0].Rows != null && domaindata.Tables[0].Rows.Count > 0)
        {
          DataRow dr = domaindata.Tables[0].Rows[0];

          if (dr["PrivateLabelID"] != DBNull.Value)
          {
            int.TryParse(dr["PrivateLabelID"].ToString(), out _privateLabelID);
          }

          if (dr["shopper_id"] != DBNull.Value)
          {
            _shopperid = dr["shopper_id"].ToString();
          }

          if (dr["IsSmartDomain"] != DBNull.Value)
          {
            bool.TryParse(dr["IsSmartDomain"].ToString(), out _isSmartDomain);
          }

          if (dr["IsMobilized"] != DBNull.Value)
          {
            bool.TryParse(dr["IsMobilized"].ToString(), out _isMobilized);
          }

          if (dr["IsActive"] != DBNull.Value)
          {
            bool.TryParse(dr["IsActive"].ToString(), out _isActive);
          }

          if (dr["ExpirationDate"] != DBNull.Value)
          {
            DateTime.TryParse(dr["ExpirationDate"].ToString(), out _expirationDate);
          }

          if (dr["DomainID"] != DBNull.Value)
          {
            int.TryParse(dr["DomainID"].ToString(), out _domainId);
          }

          if (dr["TLDID"] != DBNull.Value)
          {
            int.TryParse(dr["TLDID"].ToString(), out _tldid);
          }

          if (dr["IsProxied"] != DBNull.Value)
          {
            int.TryParse(dr["IsProxied"].ToString(), out _isProxied);
          }

          if (dr["status"] != DBNull.Value)
          {
            int.TryParse(dr["status"].ToString(), out _status);
          }

          if (dr.Table.Columns.Contains("IsPremiumDomain"))
          {
            if (dr["IsPremiumDomain"] != DBNull.Value)
            {
              bool.TryParse(dr["IsPremiumDomain"].ToString(), out _isPremiumDomain);
            }
          }

          if (dr.Table.Columns.Contains("PremiumVendorMinPrice"))
          {
            if (dr["PremiumVendorMinPrice"] != DBNull.Value)
            {
              int.TryParse(dr["PremiumVendorMinPrice"].ToString(), out _premiumVendorMinPrice);
            }
          }

          if (dr.Table.Columns.Contains("PremiumvendorMaxPrice"))
          {
            if (dr["PremiumvendorMaxPrice"] != DBNull.Value)
            {
              int.TryParse(dr["PremiumvendorMaxPrice"].ToString(), out _premiumVendorMaxPrice);
            }
          }

          if (dr.Table.Columns.Contains("PremiumVendorREcommendedPrice"))
          {
            if (dr["PremiumVendorREcommendedPrice"] != DBNull.Value)
            {
              int.TryParse(dr["PremiumVendorREcommendedPrice"].ToString(), out _premiumVendorRecPrice);
            }
          }

          if (dr.Table.Columns.Contains("Premiumuserlistprice"))
          {
            if (dr["Premiumuserlistprice"] != DBNull.Value)
            {
              int.TryParse(dr["Premiumuserlistprice"].ToString(), out _premiumUserListPrice);
            }
          }

          if (dr["IsAuction"] != DBNull.Value)
          {
            bool.TryParse(dr["IsAuction"].ToString(), out _isAuction);
          }

          if (dr.Table.Columns.Contains("AuctionPrice"))
          {
            if (dr["AuctionPrice"] != DBNull.Value)
            {
              decimal.TryParse(dr["AuctionPrice"].ToString(), out _auctionPrice);
            }
          }

          if (dr.Table.Columns.Contains("AuctionTypeID"))
          {
            if (dr["AuctionTypeID"] != DBNull.Value)
            {
              int.TryParse(dr["AuctionTypeID"].ToString(), out _auctionTypeId);
            }
          }

          if (dr.Table.Columns.Contains("AuctionEndTime"))
          {
            if (dr["AuctionEndTime"] != DBNull.Value)
            {
              DateTime.TryParse(dr["AuctionEndTime"].ToString(), out _auctionEndTime);
            }
          }

          if (dr.Table.Columns.Contains("XfrAwayDate"))
          {
            if (dr["XfrAwayDate"] != DBNull.Value)
            {
              DateTime.TryParse(dr["XfrAwayDate"].ToString(), out _xfrAwayDate);
            }
          }

          if (dr.Table.Columns.Contains("XfrAwayDateUpdateReason"))
          {
            if (dr["XfrAwayDateUpdateReason"] != DBNull.Value)
            {
              int.TryParse(dr["XfrAwayDateUpdateReason"].ToString(), out _xfrAwayDateUpdateReason);
            }
          }

          if (_xfrAwayDate > DateTime.Now && (_xfrAwayDateUpdateReason == 4 || _xfrAwayDateUpdateReason == 5))
          {
            _is60daylock = true;
          }

          if (dr.Table.Columns.Contains("CreateDate"))
          {
            if (dr["CreateDate"] != DBNull.Value)
            {
              DateTime.TryParse(dr["CreateDate"].ToString(), out _createDate);
            }
          }

          if (dr.Table.Columns.Contains("hasActiveSuspectTerms"))
          {
            if (dr["hasActiveSuspectTerms"] != DBNull.Value)
            {
              bool.TryParse(dr["hasActiveSuspectTerms"].ToString(), out _hasActiveSuspectTerms);
            }
          }
        }

        if (domaindata.Tables.Count > 1 && domaindata.Tables[1].Rows != null && domaindata.Tables[1].Rows.Count > 0)
        {
          DataRow drPdDomain = domaindata.Tables[1].Rows[0];

          if (drPdDomain.Table.Columns.Contains("PdDomainId"))
          {
            if (drPdDomain["PdDomainId"] != DBNull.Value)
            {
              int.TryParse(drPdDomain["PdDomainId"].ToString(), out _pdDomainId);
            }
          }

          if (drPdDomain.Table.Columns.Contains("DomainName"))
          {
            if (drPdDomain["DomainName"] != DBNull.Value)
            {
              _domainName = drPdDomain["PdDomainId"].ToString();
            }
          }

          if (drPdDomain.Table.Columns.Contains("ClientId"))
          {
            if (drPdDomain["ClientId"] != DBNull.Value)
            {
              int.TryParse(drPdDomain["ClientId"].ToString(), out _clientId);
            }
          }

          if (drPdDomain.Table.Columns.Contains("Client"))
          {
            if (drPdDomain["Client"] != DBNull.Value)
            {
              _client = drPdDomain["Client"].ToString();
            }
          }

          if (drPdDomain.Table.Columns.Contains("AdultClient"))
          {
            if (drPdDomain["AdultClient"] != DBNull.Value)
            {
              _adultClient = drPdDomain["AdultClient"].ToString();
            }
          }

          if (drPdDomain.Table.Columns.Contains("Channel"))
          {
            if (drPdDomain["Channel"] != DBNull.Value)
            {
              int.TryParse(drPdDomain["Channel"].ToString(), out _channel);
            }
          }

          if (drPdDomain.Table.Columns.Contains("Pkid"))
          {
            if (drPdDomain["Pkid"] != DBNull.Value)
            {
              int.TryParse(drPdDomain["Pkid"].ToString(), out _pkid);
            }
          }

          if (drPdDomain.Table.Columns.Contains("PkidTemplateId"))
          {
            if (drPdDomain["PkidTemplateId"] != DBNull.Value)
            {
              _pkidTemplateId = drPdDomain["PkidTemplateId"].ToString();
            }
          }

          if (drPdDomain.Table.Columns.Contains("AdultStatusMap"))
          {
            if (drPdDomain["AdultStatusMap"] != DBNull.Value)
            {
              int.TryParse(drPdDomain["AdultStatusMap"].ToString(), out _adultStatusMap);
            }
          }

          if (drPdDomain.Table.Columns.Contains("TemplateId"))
          {
            if (drPdDomain["TemplateId"] != DBNull.Value)
            {
              _templateId = drPdDomain["TemplateId"].ToString();
            }
          }

          if (drPdDomain.Table.Columns.Contains("SubCategoryTypeId"))
          {
            if (drPdDomain["SubCategoryTypeId"] != DBNull.Value)
            {
              int.TryParse(drPdDomain["SubCategoryTypeId"].ToString(), out _subCategoryTypeId);
            }
          }

          if (drPdDomain.Table.Columns.Contains("PatKeywords"))
          {
            if (drPdDomain["PatKeywords"] != DBNull.Value)
            {
              _patKeywords = drPdDomain["PatKeywords"].ToString();
            }
          }

          if (drPdDomain.Table.Columns.Contains("InvalidWhoIsStatus"))
          {
            if (drPdDomain["InvalidWhoIsStatus"] != DBNull.Value)
            {
              int.TryParse(drPdDomain["InvalidWhoIsStatus"].ToString(), out _invalidWhoIsStatus);
            }
          }

          if (drPdDomain.Table.Columns.Contains("HasClicks"))
          {
            if (drPdDomain["HasClicks"] != DBNull.Value)
            {
              int.TryParse(drPdDomain["HasClicks"].ToString(), out _hasClicks);
            }
          }

          if (drPdDomain.Table.Columns.Contains("HasProdRev"))
          {
            if (drPdDomain["HasProdRev"] != DBNull.Value)
            {
              int.TryParse(drPdDomain["HasProdRev"].ToString(), out _hasProdRev);
            }
          }

          if (drPdDomain.Table.Columns.Contains("RecordUpdated"))
          {
            if (drPdDomain["RecordUpdated"] != DBNull.Value)
            {
              DateTime.TryParse(drPdDomain["RecordUpdated"].ToString(), out _recordUpdated);
            }
          }
        }

        if (domaindata.Tables.Count > 2 && domaindata.Tables[2].Rows != null && domaindata.Tables[2].Rows.Count > 0)
        {
          DataRow drCashParkingPdDomain = domaindata.Tables[2].Rows[0];

          if (drCashParkingPdDomain.Table.Columns.Contains("TemplateId"))
          {
            if (drCashParkingPdDomain["TemplateId"] != DBNull.Value)
            {
              _cashParkingTemplateId = drCashParkingPdDomain["TemplateId"].ToString();
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("ActiveFlag"))
          {
            if (drCashParkingPdDomain["ActiveFlag"] != DBNull.Value)
            {
              bool.TryParse(drCashParkingPdDomain["ActiveFlag"].ToString(), out _cashParkingActiveFlag);
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("AdultMode"))
          {
            if (drCashParkingPdDomain["AdultMode"] != DBNull.Value)
            {
              int.TryParse(drCashParkingPdDomain["AdultMode"].ToString(), out _cashParkingAdultMode);
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("Keywords"))
          {
            if (drCashParkingPdDomain["Keywords"] != DBNull.Value)
            {
              _cashParkingKeywords = drCashParkingPdDomain["Keywords"].ToString();
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("ThemeId"))
          {
            if (drCashParkingPdDomain["ThemeId"] != DBNull.Value)
            {
              int.TryParse(drCashParkingPdDomain["ThemeId"].ToString(), out _cashParkingThemeId);
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("TemplateTypeCode"))
          {
            if (drCashParkingPdDomain["TemplateTypeCode"] != DBNull.Value)
            {
              int.TryParse(drCashParkingPdDomain["TemplateTypeCode"].ToString(), out _cashParkingTemplateTypeCode);
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("SubCategoryTypeId"))
          {
            if (drCashParkingPdDomain["SubCategoryTypeId"] != DBNull.Value)
            {
              int.TryParse(drCashParkingPdDomain["SubCategoryTypeId"].ToString(), out _cashParkingSubCategoryTypeId);
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("TrpListingStatusCode"))
          {
            if (drCashParkingPdDomain["TrpListingStatusCode"] != DBNull.Value)
            {
              int.TryParse(drCashParkingPdDomain["TrpListingStatusCode"].ToString(), out _cashParkingTrpListingStatusCode);
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("ClientId"))
          {
            if (drCashParkingPdDomain["ClientId"] != DBNull.Value)
            {
              int.TryParse(drCashParkingPdDomain["ClientId"].ToString(), out _cashParkingClientId);
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("Client"))
          {
            if (drCashParkingPdDomain["Client"] != DBNull.Value)
            {
              _cashParkingClient = drCashParkingPdDomain["Client"].ToString();
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("AdultClient"))
          {
            if (drCashParkingPdDomain["AdultClient"] != DBNull.Value)
            {
              _cashParkingAdultClient = drCashParkingPdDomain["AdultClient"].ToString();
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("Channel"))
          {
            if (drCashParkingPdDomain["Channel"] != DBNull.Value)
            {
              int.TryParse(drCashParkingPdDomain["Channel"].ToString(), out _cashParkingChannel);
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("PopundersEnabled"))
          {
            if (drCashParkingPdDomain["PopundersEnabled"] != DBNull.Value)
            {
              bool.TryParse(drCashParkingPdDomain["PopundersEnabled"].ToString(), out _cashParkingPopundersEnabled);
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("ZeroclickEnabled"))
          {
            if (drCashParkingPdDomain["ZeroclickEnabled"] != DBNull.Value)
            {
              bool.TryParse(drCashParkingPdDomain["ZeroclickEnabled"].ToString(), out _cashParkingZeroclickEnabled);
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("InquireLinkEnabled"))
          {
            if (drCashParkingPdDomain["InquireLinkEnabled"] != DBNull.Value)
            {
              int.TryParse(drCashParkingPdDomain["InquireLinkEnabled"].ToString(), out _cashParkingInquireLinkEnabled);
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("CustomListingText"))
          {
            if (drCashParkingPdDomain["CustomListingText"] != DBNull.Value)
            {
              _customListingText = drCashParkingPdDomain["CustomListingText"].ToString();
            }
          }

          if (drCashParkingPdDomain.Table.Columns.Contains("CustomListingLink"))
          {
            if (drCashParkingPdDomain["CustomListingLink"] != DBNull.Value)
            {
              _customListingLink = drCashParkingPdDomain["CustomListingLink"].ToString();
            }
          }
        }
      }
    }
  }
}
