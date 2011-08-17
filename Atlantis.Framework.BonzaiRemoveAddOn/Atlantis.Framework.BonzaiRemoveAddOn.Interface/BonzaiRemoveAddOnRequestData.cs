using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonzaiRemoveAddOn.Interface
{
  public class BonzaiRemoveAddOnRequestData : RequestData
  {
    #region Properties

    public string AccountUid { get; private set; }
    public string AddOnType { get; private set; }
    public string AttributeUid { get; private set; }

    #endregion

    public BonzaiRemoveAddOnRequestData(string shopperId,
                                        string sourceUrl,
                                        string orderIo,
                                        string pathway,
                                        int pageCount,
                                        string accountUid,
                                        string attributeUid,
                                        string addOnType)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      AccountUid = accountUid;
      AddOnType = addOnType;
      AttributeUid = attributeUid;
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in BonzaiRemoveAddOnRequestData");     
    }
  }
}
