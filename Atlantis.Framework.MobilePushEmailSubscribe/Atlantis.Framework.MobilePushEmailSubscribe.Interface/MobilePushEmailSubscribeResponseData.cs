using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobilePushEmailSubscribe.Interface
{
  public class MobilePushEmailSubscribeResponseData : IResponseData
  {
    private AtlantisException AtlantisException { get; set; }

    public long SubscriptionId { get; private set; }

    public MobilePushEmailSubscribeResponseData(long subscriptionId)
    {
      SubscriptionId = subscriptionId;
    }

    public MobilePushEmailSubscribeResponseData(RequestData requestData, Exception ex)
    {
      AtlantisException = new AtlantisException(requestData,
                                                MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                ex.Message,
                                                ex.StackTrace);
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }
  }
}
