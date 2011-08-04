using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BPBlogSubscriberAdd.Interface
{
  public class BPBlogSubscriberAddRequestData : RequestData
  {
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public bool Confirmed { get; private set; }

    public BPBlogSubscriberAddRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      string email, string firstName, string lastName, bool confirmed)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Email = email;
      FirstName = firstName;
      LastName = lastName;
      Confirmed = confirmed;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("BPBlogSubscriberAdd is not a cacheable request.");
    }
  }
}
