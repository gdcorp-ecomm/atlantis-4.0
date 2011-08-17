using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonzaiApplyAddOn.Interface
{
  public class BonzaiApplyAddOnRequestData : RequestData
  {
    #region Properties

    public string AccountUid { get; private set; }
    public string AddOnType { get; private set; }

    #endregion

    public BonzaiApplyAddOnRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  string accountUid,
                                  string addOnType)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      AccountUid = accountUid;
      AddOnType = addOnType;
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in BonzaiApplyAddOnRequestData");
    }
  }
}
