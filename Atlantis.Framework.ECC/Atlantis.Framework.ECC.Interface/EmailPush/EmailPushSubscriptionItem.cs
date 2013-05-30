using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Ecc.Interface
{
  [DataContract]
  public class EmailPushSubscriptionItem
  {
    [DataMember(Name="subscription_id")]
    public int SubscriptionId { get; set; }

    [DataMember(Name="user_id")]
    public string EmailAddress { get; set; }

    [DataMember(Name="callback_url")]
    public string CallbackUrl { get; set; }

    [DataMember(Name="notification_info")]
    public string NotificationInfo { get; set; }

    [DataMember(Name="added")]
    private string Added { get; set; }

    public DateTime DateAdded
    {
      get
      {
        DateTime temp;
        if (!DateTime.TryParse(Added, out temp))
        {
        }
        return temp;
      }
    }


    [DataMember(Name="modified")]
    private string Modified { get; set; }

    public DateTime DateModified
    {
      get
      {
        DateTime temp;
        if (!DateTime.TryParse(Modified, out temp))
        {
        }
        return temp;
      }
    }

    [DataMember(Name="mobile")]
    public bool IsMobile { get; set; }
  }
}
