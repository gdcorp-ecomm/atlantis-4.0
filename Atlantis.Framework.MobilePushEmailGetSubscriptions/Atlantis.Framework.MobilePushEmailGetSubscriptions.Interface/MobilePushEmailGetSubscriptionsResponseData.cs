using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.SessionCache;


namespace Atlantis.Framework.MobilePushEmailGetSub.Interface
{
  public class MobilePushEmailGetSubscriptionsResponseData : EccResponseDataBase<EmailPushSubscriptions>, ISessionSerializableResponse
  {
    private AtlantisException AtlantisException { get; set; }
    public List<EmailPushSubscriptionItem> Subscriptions
    {
      get
      {
        List<EmailPushSubscriptionItem> data = null;
        try
        {
          if (this.Response.Item.Results[0].Count > 0)
          {
            data = new List<EmailPushSubscriptionItem>(this.Response.Item.Results[0]);
          }
        }
        catch (Exception ex)
        {
        }

        return data;
      }
    }

    public bool LoginExists
    {
      get
      {
        bool hasSubs = (Subscriptions != null);
        return hasSubs && Subscriptions.Count > 0;
      }
    }

    public MobilePushEmailGetSubscriptionsResponseData(string jsonResponse)
      : base(jsonResponse)
    {
    }

    public MobilePushEmailGetSubscriptionsResponseData(AtlantisException ex)
    {
      AtlantisException = ex;
    }

    public MobilePushEmailGetSubscriptionsResponseData(RequestData requestData, Exception ex)
    {
      AtlantisException = new AtlantisException(requestData,
                                                MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                ex.Message,
                                                ex.StackTrace);
    }

    public static IResponseData FromAtlantisException(AtlantisException exception)
    {
      return new MobilePushEmailGetSubscriptionsResponseData(exception);
    }

    public string SerializeSessionData()
    {
      throw new NotImplementedException();
    }

    public void DeserializeSessionData(string sessionData)
    {
      throw new NotImplementedException();
    }

  }
}
