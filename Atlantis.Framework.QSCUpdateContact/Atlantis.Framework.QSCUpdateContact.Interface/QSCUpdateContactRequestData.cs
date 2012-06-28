using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSCUpdateContact.Interface
{
  public class QSCUpdateContactRequestData : RequestData
  {
    public QSCUpdateContactRequestData(string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountUid,
      string invoiceId,
      string nickName,
      string firstName,
      string middleInitial,
      string lastName,
      string companyName,
      string addressLine1,
      string addressLine2,
      string city,
      string regionCode,
      string postalCode,
      string countryCode,
      string email,
      string phoneNumber,
      string contactType
      ) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      OrderContact = new orderContact();


      AccountUid = accountUid;
      InvoiceId = invoiceId;
      
      OrderContact.nickName = nickName;
      OrderContact.firstName = firstName;
      OrderContact.midInit = middleInitial;
      OrderContact.lastName = lastName;
      OrderContact.companyName = companyName;
      OrderContact.addressLine1 = addressLine1;
      OrderContact.addressLine2 = addressLine2;
      OrderContact.city = city;
      OrderContact.regionCode = regionCode;
      OrderContact.postalCode = postalCode;
      OrderContact.countryCode = countryCode;
      OrderContact.email = email;
      OrderContact.phoneNumber = phoneNumber;
      OrderContact.contactType = contactType;
    }

    public string AccountUid { get; set; }
    public string InvoiceId { get; set; }
    public orderContact OrderContact { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("QSCUpdateContact is not a cacheable request.");
    }

    #endregion
  }
}
