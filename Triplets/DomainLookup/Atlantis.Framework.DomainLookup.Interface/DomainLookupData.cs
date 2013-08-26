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

    private void ParseDomainLookupResponse(DataSet domaindata)
    {
      if (domaindata != null && domaindata.Tables.Count > 0 && domaindata.Tables[0].Rows != null && domaindata.Tables[0].Rows.Count > 0)
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
    }
  }
}
