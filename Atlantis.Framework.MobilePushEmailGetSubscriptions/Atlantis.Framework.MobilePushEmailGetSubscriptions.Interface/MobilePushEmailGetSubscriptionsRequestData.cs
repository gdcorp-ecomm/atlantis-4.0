using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.MobilePushEmailGetSub.Interface
{
    public class MobilePushEmailGetSubscriptionsRequestData : RequestData
    {
        public MobilePushEmailGetSubscriptionsRequestData(string email, string subscriptionId, string shopperId, string sourceURL, string orderId, string pathway, int pageCount) 
            : base(shopperId, sourceURL, orderId, pathway, pageCount)
        {
            Email = email;
            SubscriptionId = subscriptionId;
        }

        public override string GetCacheMD5()
        {
            throw new Exception("MobilePushEmailGetSubscriptions is not a cacheable request.");
        }

        public string Email { get; private set; }

        public string SubscriptionId { get; set; }
    }
}
