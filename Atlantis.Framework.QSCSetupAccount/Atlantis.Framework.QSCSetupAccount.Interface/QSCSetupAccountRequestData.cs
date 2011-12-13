using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCSetupAccount.Interface
{
  public class QSCSetupAccountRequestData : RequestData
  {

    public string AccountUID { get; set; }
    public string DomainName { get; set; }
    public string EmailAddress { get; set; }
    public string CompanyName { get; set; }
    public string ThemeID { get; set; }

    public QSCSetupAccountRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  string accountUID,
                                  string domainName,
                                  string emailAddress,
                                  string companyName,
                                  string themeID
      )
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      RequestTimeout = new TimeSpan(0, 1, 0);
      AccountUID = accountUID;
      DomainName = domainName;
      EmailAddress = emailAddress;
      CompanyName = companyName;
      ThemeID = themeID;    
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in QSCSetupAccountRequestData");     
    }


  }
}
