using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Atlantis.Framework.MobilePushEmailGetSubscriptions.Interface.Objects
{

    /*
     * JSON from http://ecctest.devlhvm03.stgwbe.com/rim/index.php?action=getSubscriptions&login=andy@123-weight.com
     * 
       {
        subscription_id: "305",
        user_id: "andy@123-weight.com",
        callback_url: "https://mob.test.glbt1.gdg/EmailMobilePushService/PushNotification.ashx?action=Notification&login=andy@123-weight.com",
        notification_info: "reggy4",
        added: "2012-12-03 14:51:11",
        modified: "2012-12-03 14:51:11"
        },
     */
    [DataContract]
    public class PushEmailSubscription
    {
        [DataMember(Name="subscription_id") ]
        public string SubscriptionID { get; set; }
        [DataMember(Name = "user_id")]
        public string UserID { get; set; }
        [DataMember(Name = "callback_url")]
        public string CallbackUrl { get; set; }
        [DataMember(Name = "notification_info")]
        public string NotificationInfo { get; set; }

        // TODO "added", "modified" date/times (not sure how to do DataContract yet)
    }



}
