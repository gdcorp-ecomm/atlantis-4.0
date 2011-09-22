using System.Runtime.Serialization;

namespace Atlantis.Framework.Auction.Interface {
  /// <summary>
  /// Represents the Member Information for an Auction User
  /// </summary>
  [DataContract]
  public class AuctionMemberInfo {

    /// <summary>
    /// Initializes a new instance of the <see cref="AuctionMemberInfo"/> class.
    /// </summary>
    public AuctionMemberInfo()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuctionMemberInfo"/> class.
    /// </summary>
    /// <param name="memberId">The member id.</param>
    /// <param name="shopperId">The shopper id.</param>
    /// <param name="statusCodeId">The status code id.</param>
    /// <param name="hasAdultFilter">if set to <c>true</c> [has adult filter].</param>
    /// <param name="isGoodAsGoldEnabled">if set to <c>true</c> [is good as gold enabled].</param>
    /// <param name="privateLabelId">The private label id.</param>
    public AuctionMemberInfo(string memberId, string shopperId, int statusCodeId, bool hasAdultFilter, bool isGoodAsGoldEnabled, string privateLabelId)
    {
      MemberId = memberId;
      ShopperId = shopperId;
      StatusCodeId = statusCodeId;
      HasAdultFilter = hasAdultFilter;
      IsGoodAsGoldEnabled = isGoodAsGoldEnabled;
      PrivateLabelId = privateLabelId;
    }

    /// <summary>
    /// Gets or sets the member id.
    /// </summary>
    /// <value>
    /// The member id.
    /// </value>
    [DataMember]
    public string MemberId { get; set; }
    
    /// <summary>
    /// Gets or sets the shopper id.
    /// </summary>
    /// <value>
    /// The shopper id.
    /// </value>
    [DataMember]
    public string ShopperId { get; set; }
    
    /// <summary>
    /// Gets or sets the status code id.
    /// 14 will get set by Tier 2 for users committing fraud 
    /// 27 happens automatically if they have items more than 5 days unpaid, reverts to 1 on payment
    /// </summary>
    /// <value>
    /// The status code id.  1 - active; 14 - banned; 27 - blocked (for outstanding payment); 28 - never block, 29 - inactive
    /// </value>
    [DataMember]
    public int StatusCodeId { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether this instance has adult filter.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has adult filter; otherwise, <c>false</c>.
    /// </value>
    [DataMember]
    public bool HasAdultFilter { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether this instance is good as gold enabled.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is good as gold enabled; otherwise, <c>false</c>.
    /// </value>
    [DataMember]
    public bool IsGoodAsGoldEnabled { get; set; }

    /// <summary>
    /// Gets or sets the private label id.
    /// </summary>
    /// <value>
    /// The private label id.
    /// </value>
    [DataMember]
    public string PrivateLabelId { get; set; }

    /// <summary>
    /// The original xml from the api call
    /// </summary>
    [DataMember]
    public string RawXml { get; set; }

    private bool? _subscriptionActive;
    public bool SubscriptionActive 
    {
      get 
      { 
        if (_subscriptionActive == null)
        {
          switch (StatusCodeId)
          {
            case 1:
            case 14:
            case 27:
            case 28:
              _subscriptionActive = true;
              break;
            default:
              _subscriptionActive = false;
              break;
          }
        }
        return _subscriptionActive.Value;
      }
    }

    private bool? _subscriptionBanned;
    public bool SubscriptionBanned 
    { 
      get 
      {
        if (_subscriptionBanned == null)
        {
          switch (StatusCodeId)
          {
            case 14:
              _subscriptionBanned = true;
              break;
            default:
              _subscriptionBanned = false;
              break;
          }
        }
        return _subscriptionBanned.Value;
      } 
    }

    private bool? _subscriptionBlocked;
    public bool SubscriptionBlocked 
    {
      get 
      {
        if (_subscriptionBlocked == null)
        {
          switch (StatusCodeId)
          {
            case 27:
              _subscriptionBlocked = true;
              break;
            default:
              _subscriptionBlocked = false;
              break;
          }
        }
        return _subscriptionBlocked.Value;
      }
    }
  }
}
