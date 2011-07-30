using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MktgSetShopperCommPrivacyHash.Interface
{
  public class MktgSetShopperCommPrivacyHashRequestData : RequestData
  {
    public int CommunicationTypeID { get; set; }
    public string PrivacyHash { get; set; }

    public MktgSetShopperCommPrivacyHashRequestData(
      string shopperId,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount,
      int communicationTypeID,
      string privacyHash)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      CommunicationTypeID = communicationTypeID;
      PrivacyHash = privacyHash;
      RequestTimeout = TimeSpan.FromSeconds(6);
    }
    
    public override string GetCacheMD5()
    {
      throw new NotImplementedException("MktgSetShopperCommPrivacyHashRequestData is not a cacheable request.");

    }
  }
}
