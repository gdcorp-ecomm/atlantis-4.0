using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDValidateTroubleTicket.Interface
{
  public class HDVDValidateTroubleTicketRequestData : RequestData
  {
    private readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(15);

    public HDVDValidateTroubleTicketRequestData(
      string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string firstName,
      string lastName,
      string emailAddress,
      string phoneNumber,
      string summary,
      string details
      ) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      FirstName = firstName;
      LastName = lastName;
      EmailAddress = emailAddress;
      PhoneNumber = phoneNumber;
      Summary = summary;
      Details = details;
      RequestTimeout = _requestTimeout;
    }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string EmailAddress { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Summary { get; set; }
    
    public string Details { get; set; }


    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("HDVDValidateTroubleTicket is not a cacheable request.");
    }

    #endregion
  }
}
