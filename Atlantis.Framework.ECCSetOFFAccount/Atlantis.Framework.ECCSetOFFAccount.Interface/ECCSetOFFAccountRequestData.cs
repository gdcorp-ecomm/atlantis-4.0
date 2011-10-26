using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCSetOFFAccount.Interface
{
  public class ECCSetOFFAccountRequestData : RequestData
  {
    public ECCSetOFFAccountRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, Guid accountUid, bool activeOnly, string password, string username, string emailAddress, int privateLabelId, string subAccount) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      ActiveOnly = activeOnly;
      Password = password;
      Username = username;
      EmailAddress = emailAddress;
      PrivateLabelId = privateLabelId;
      SubAccount = subAccount;
    }

    public string SubAccount { get; set; }

    public int PrivateLabelId { get; set; }

    /// <summary>
    /// If the email address parameter is not supplied, no change to the current email address association will occur for an existing OFF account
    /// If the email address parameter is supplied, but empty; the OFF account will have no email associated.
    /// If the email address parameter is supplied, it cannot be assigned to any other OFF account.
    /// </summary>
    public string EmailAddress { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public bool ActiveOnly { get; set; }

    public Guid AccountUid{ get; set; }

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(30);

    public override string GetCacheMD5()
    {
      throw new Exception("ECCSetOFFAccount is not a cacheable request.");
    }

  }
}
