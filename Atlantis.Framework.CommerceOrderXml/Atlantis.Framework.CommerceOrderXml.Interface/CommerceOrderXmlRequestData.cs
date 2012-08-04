using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CommerceOrderXml.Interface
{
  public class CommerceOrderXmlRequestData : RequestData
  {
    #region Properties
    private string _recentOrderId;

    public string RecentOrderId
    {
      get { return _recentOrderId; }
    }

    #endregion
    public CommerceOrderXmlRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string recentOrderId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _recentOrderId = recentOrderId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
