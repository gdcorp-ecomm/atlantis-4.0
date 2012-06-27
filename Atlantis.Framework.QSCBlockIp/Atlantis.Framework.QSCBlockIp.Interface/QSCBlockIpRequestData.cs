using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCBlockIp.Interface
{
  public class QSCBlockIpRequestData : RequestData
  {
    public QSCBlockIpRequestData(string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountUid,
      string ipAddress) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      IpAddress = ipAddress;
    }

    public string AccountUid { get; set; }
    public string IpAddress { get; set; }
    
    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("QSCBlockIp is not a cacheable request.");
    }

    #endregion
  }
}
