using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSSellerInterested.Interface
{
  public class DBSSellerInterestedRequestData : RequestData
  {
    private int _resourceId;
    private int _claimId;
    private string _managerUserId;
    private string _requestXml;
    
    public DBSSellerInterestedRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, 
    int resourceId, int claimId, string managerUserId)
      : base (shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _resourceId = resourceId;
      _claimId = claimId;
      _managerUserId = managerUserId;      
      _requestXml = RequestXml;
      RequestTimeout = TimeSpan.FromSeconds(4);
    }

    public int ResourceId
    {
      get { return _resourceId; }
      set { _resourceId = value; }
    }

    public int ClaimId
    {
      get { return _claimId; }
      set { _claimId = value; }
    }
    
    public string ManagerUserId
    {
      get { return _managerUserId; }
      set { _managerUserId = value; }
    }

    public string RequestXml
    {
      get 
      {
        return string.Format("<updates><update dbsResourceId='{0}' claimId='{1}' managerUserId='{2}' /></updates>", ResourceId.ToString(), ClaimId.ToString(), ManagerUserId);
      }
    }
    
    public override string GetCacheMD5()
    {
      throw new Exception("DBSSellerInterested is not a cacheable request.");
    }

  }
}
