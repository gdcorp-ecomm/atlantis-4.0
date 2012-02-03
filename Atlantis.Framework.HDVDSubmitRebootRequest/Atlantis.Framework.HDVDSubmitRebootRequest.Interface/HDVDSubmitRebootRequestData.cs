using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDSubmitRebootRequest.Interface
{
  public class HDVDSubmitRebootRequestData : RequestData
  {

    public Guid AccountUid { get; set; }

    public HDVDSubmitRebootRequestData(string shopperId, string sourceUrl, string orderIo, string pathway, int pageCount, string accountUid)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      if (!string.IsNullOrEmpty(accountUid))
      {
        AccountUid = new Guid(accountUid);
      }
      else
      {
        throw new ArgumentNullException(accountUid, "AccountUid cannot be null or empty!");
      }

      if (string.IsNullOrEmpty(shopperId)) {
        throw new ArgumentNullException(shopperId, "ShopperId cannot be null or empty!");
      }

    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in HDVDSubmitRebootRequestData");     
    }


  }
}
