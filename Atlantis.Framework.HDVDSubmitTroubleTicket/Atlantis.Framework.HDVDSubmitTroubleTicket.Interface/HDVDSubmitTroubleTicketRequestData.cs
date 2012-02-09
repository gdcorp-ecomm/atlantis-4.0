using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDSubmitTroubleTicket.Interface
{
  public class HDVDSubmitTroubleTicketRequestData : RequestData
  {
    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(15);
    
    public HDVDSubmitTroubleTicketRequestData(string shopperId, 
                                              string sourceURL, 
                                              string orderId, 
                                              string pathway, 
                                              int pageCount, 
                                              string accountGuid, 
                                              string custFirstName, 
                                              string custLastName, 
                                              string custEmail, 
                                              string custPhone, 
                                              string ticketTitle, 
                                              string ticketBody, 
                                              bool hasBeenRebooted, 
                                              bool grantSupportAccess) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountGuid = accountGuid;
      CustFirstName = custFirstName;
      CustLastName = custLastName;
      CustEmail = custEmail;
      CustPhone = custPhone;
      TicketTitle = ticketTitle;
      TicketBody = ticketBody;
      HasBeenRebooted = hasBeenRebooted;
      GrantSupportAccess = grantSupportAccess;
      RequestTimeout = _requestTimeout;
    }


    public string AccountGuid { get; set; }
    
    public string CustFirstName { get; set; }
    
    public string CustLastName { get; set; }
    
    public string CustEmail { get; set; }
    
    public string CustPhone { get; set; }
    
    public string TicketTitle { get; set; }
    
    public string TicketBody { get; set; }
    
    public bool HasBeenRebooted { get; set; }
    
    public bool GrantSupportAccess { get; set; }



    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("HDVDSubmitTroubleTicket is not a cacheable request.");
    }

    #endregion
  }
}
