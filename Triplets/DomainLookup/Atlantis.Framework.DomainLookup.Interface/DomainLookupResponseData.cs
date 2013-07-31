using Atlantis.Framework.Interface;
using System;
using System.Data;
using System.Xml.Linq;

namespace Atlantis.Framework.DomainLookup.Interface
{
    public class DomainLookupResponseData : IResponseData
    {
        protected int privateLabelID = 1;
        protected string shopperid = "";
        protected bool isSmartDomain = false;
        protected bool isMobilized = false;
        protected bool isActive = false;
        protected DateTime expirationDate = DateTime.MinValue;
        protected int domainId = 0;
        protected int tldid = 0;
        protected int isProxied = 0;
        protected int status = 0;
        protected bool isPremiumDomain = false;
        protected int premiumVendorMinPrice = 0;
        protected int premiumVendorMaxPrice = 0;
        protected int premiumVendorRecPrice = 0;
        protected int premiumUserListPrice = 0;
        protected bool isAuction = false;
        protected decimal auctionPrice = 0;
        protected int auctionTypeId = 0;
        protected DateTime auctionEndTime = DateTime.MinValue;
        protected DateTime xfrDate = DateTime.MinValue;
        protected int xfrDateReason = 0;
        protected bool is60daylock = false;
        protected DateTime createDate = DateTime.MinValue;
        protected bool hasActiveSuspectTerms = false;
        protected AtlantisException atlantisEx = null;

        // Sample static method for creating the response (ref Clean Code)
        public static DomainLookupResponseData FromData(object data, AtlantisException atlantisException)
        {
            return new DomainLookupResponseData(data, atlantisException);
        }

        // Sample private constructor
        private DomainLookupResponseData(object data, AtlantisException atlantisException)
        {
            
            DataSet ds = (DataSet)data;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["PrivateLabelID"] != DBNull.Value)
                {
                    int.TryParse(dr["PrivateLabelID"].ToString(), out privateLabelID);
                }

                if (dr["shopper_id"] != DBNull.Value)
                {
                    shopperid = dr["shopper_id"].ToString();
                }

                if (dr["IsSmartDomain"] != DBNull.Value)
                {
                    bool.TryParse(dr["IsSmartDomain"].ToString(), out isSmartDomain);
                }

                if (dr["IsMobilized"] != DBNull.Value)
                {
                    bool.TryParse(dr["IsMobilized"].ToString(), out isMobilized);
                }

                if (dr["IsActive"] != DBNull.Value)
                {
                    bool.TryParse(dr["IsActive"].ToString(), out isActive);
                }

                if (dr["ExpirationDate"] != DBNull.Value)
                {
                    DateTime.TryParse(dr["ExpirationDate"].ToString(), out expirationDate);
                }

                if (dr["DomainID"] != DBNull.Value)
                {
                    int.TryParse(dr["DomainID"].ToString(), out domainId);
                }

                if (dr["TLDID"] != DBNull.Value)
                {
                    int.TryParse(dr["TLDID"].ToString(), out tldid);
                }

                if (dr["IsProxied"] != DBNull.Value)
                {
                    int.TryParse(dr["IsProxied"].ToString(), out isProxied);
                }

                if (dr["status"] != DBNull.Value)
                {
                    int.TryParse(dr["status"].ToString(), out status);
                }

                if (dr.Table.Columns.Contains("IsPremiumDomain"))
                {
                    if (dr["IsPremiumDomain"] != DBNull.Value)
                    {
                        bool.TryParse(dr["IsPremiumDomain"].ToString(), out isPremiumDomain);
                    }
                }

                if (dr.Table.Columns.Contains("PremiumVendorMinPrice"))
                {
                    if (dr["PremiumVendorMinPrice"] != DBNull.Value)
                    {
                        int.TryParse(dr["PremiumVendorMinPrice"].ToString(), out premiumVendorMinPrice);
                    }
                }

                if (dr.Table.Columns.Contains("PremiumvendorMaxPrice"))
                {
                    if (dr["PremiumvendorMaxPrice"] != DBNull.Value)
                    {
                        int.TryParse(dr["PremiumvendorMaxPrice"].ToString(), out premiumVendorMaxPrice);
                    }
                }

                if (dr.Table.Columns.Contains("PremiumVendorREcommendedPrice"))
                {
                    if (dr["PremiumVendorREcommendedPrice"] != DBNull.Value)
                    {
                        int.TryParse(dr["PremiumVendorREcommendedPrice"].ToString(), out premiumVendorRecPrice);
                    }
                }

                if (dr.Table.Columns.Contains("Premiumuserlistprice"))
                {
                    if (dr["Premiumuserlistprice"] != DBNull.Value)
                    {
                        int.TryParse(dr["Premiumuserlistprice"].ToString(), out premiumUserListPrice);
                    }
                }

                if (dr["IsAuction"] != DBNull.Value)
                {
                    bool.TryParse(dr["IsAuction"].ToString(), out isAuction);
                }

                if (dr.Table.Columns.Contains("AuctionPrice"))
                {
                    if (dr["AuctionPrice"] != DBNull.Value)
                    {
                        decimal.TryParse(dr["AuctionPrice"].ToString(), out auctionPrice);
                    }
                }

                if (dr.Table.Columns.Contains("AuctionTypeID"))
                {
                    if (dr["AuctionTypeID"] != DBNull.Value)
                    {
                        int.TryParse(dr["AuctionTypeID"].ToString(), out auctionTypeId);
                    }
                }

                if (dr.Table.Columns.Contains("AuctionEndTime"))
                {
                    if (dr["AuctionEndTime"] != DBNull.Value)
                    {
                        DateTime.TryParse(dr["AuctionEndTime"].ToString(), out auctionEndTime);
                    }
                }

                if (dr.Table.Columns.Contains("XfrAwayDate"))
                {
                    if (dr["XfrAwayDate"] != DBNull.Value)
                    {
                        DateTime.TryParse(dr["XfrAwayDate"].ToString(), out xfrDate);
                    }
                }

                if (dr.Table.Columns.Contains("XfrAwayDateUpdateReason"))
                {
                    if (dr["XfrAwayDateUpdateReason"] != DBNull.Value)
                    {
                        int.TryParse(dr["XfrAwayDateUpdateReason"].ToString(), out xfrDateReason);
                    }
                }

                if (xfrDate > DateTime.Now && (xfrDateReason == 4 || xfrDateReason == 5))
                {
                    is60daylock = true;
                }

                if (dr.Table.Columns.Contains("CreateDate"))
                {
                    if (dr["CreateDate"] != DBNull.Value)
                    {
                        DateTime.TryParse(dr["CreateDate"].ToString(), out createDate);
                    }
                }

                if (dr.Table.Columns.Contains("hasActiveSuspectTerms"))
                {
                    if (dr["hasActiveSuspectTerms"] != DBNull.Value)
                    {
                        bool.TryParse(dr["hasActiveSuspectTerms"].ToString(), out hasActiveSuspectTerms);
                    }
                }

                if (atlantisException != null)
                {
                    this.atlantisEx = atlantisException;
                }
            }
        }

        public string ToXML()
        {
            //TODO: Serialize the object to XML? -or- come up with XML writer to output XML?
            return "";
        }

        public AtlantisException GetException()
        {
            return this.atlantisEx;
        }

        public int PrivateLabelID { get { return this.privateLabelID; } }
        public string Shopperid { get { return this.shopperid; } }
        public bool IsSmartDomain { get { return this.isSmartDomain; } }
        public bool IsMobilized { get { return this.isMobilized; } }
        public bool IsActive { get { return this.isActive; } }
        public DateTime ExpirationDate { get { return this.expirationDate; } }
        public int DomainID { get { return this.domainId; } }
        public int TldID { get { return this.tldid; } }
        public int IsProxied { get { return this.isProxied; } }
        public int Status { get { return this.status; } }
        public bool IsPremiumDomain { get { return this.isPremiumDomain; } }
        public int PremiumVendorMinPrice { get { return this.premiumVendorMinPrice; } }
        public int PremiumVendorMaxPrice { get { return this.premiumVendorMaxPrice; } }
        public int PremiumVendorRecPrice { get { return this.premiumVendorRecPrice; } }
        public int PremiumUserListPrice { get { return this.premiumUserListPrice; } }
        public bool IsAuction { get { return this.isAuction; } }
        public decimal AuctionPrice { get { return this.auctionPrice; } }
        public int AuctionTypeID { get { return this.auctionTypeId; } }
        public DateTime AuctionEndTime { get { return this.auctionEndTime; } }
        public DateTime XfrAwayDate { get { return this.xfrDate; } }
        public int XfrAwayDateUpdateReason { get { return this.xfrDateReason; } }
        public bool is60dayLock { get { return this.is60daylock; } }
        public DateTime CreateDate { get { return this.createDate; } }
        public bool HasSuspectTerms { get { return this.hasActiveSuspectTerms; } }
        public AtlantisException AtlantisEx { get { return this.atlantisEx; } }
    }
}
