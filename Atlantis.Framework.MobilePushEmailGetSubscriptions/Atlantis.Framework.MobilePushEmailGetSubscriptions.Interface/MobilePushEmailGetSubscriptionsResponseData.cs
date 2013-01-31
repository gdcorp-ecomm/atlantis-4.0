using System.Reflection;
using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.MobilePushEmailGetSubscriptions.Interface.Objects;


namespace Atlantis.Framework.MobilePushEmailGetSub.Interface
{
    public class MobilePushEmailGetSubscriptionsResponseData : IResponseData
    {
        private AtlantisException AtlantisException { get; set; }
        public PushEmailSubscription[] Subscriptions { get; set; }
        public bool LoginExists { get; set; }

        public MobilePushEmailGetSubscriptionsResponseData(PushEmailSubscription[] subscriptions)
        {
            Subscriptions = subscriptions;
            if (subscriptions!=null)
                LoginExists = true;
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

        public string ToXML()
        {
            throw new NotImplementedException();
        }

        public AtlantisException GetException()
        {
            return AtlantisException;
        }

    }
}
