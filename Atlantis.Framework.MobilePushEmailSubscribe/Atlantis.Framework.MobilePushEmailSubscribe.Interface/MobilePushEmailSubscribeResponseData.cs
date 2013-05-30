using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.MobilePushEmailSubscribe.Interface
{
  public class MobilePushEmailSubscribeResponseData : EccResponseDataBase<PushSubscribeDetails>, ISessionSerializableResponse
  {
    private AtlantisException AtlantisException { get; set; }

    private string _subscriptionId = null;
    public string SubscriptionId
    {
      get
      {
        return _subscriptionId ?? (_subscriptionId = this.Response.Item.Results[0].SubscriptionId ?? "-1");
      }
      private set { _subscriptionId = value; }
    }

    public MobilePushEmailSubscribeResponseData(string resultJson)
      : base(resultJson)
	  {
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

    #region Implementation of ISessionSerializableResponse

    public string SerializeSessionData()
    {
      throw new NotImplementedException();
    }

    public void DeserializeSessionData(string sessionData)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
