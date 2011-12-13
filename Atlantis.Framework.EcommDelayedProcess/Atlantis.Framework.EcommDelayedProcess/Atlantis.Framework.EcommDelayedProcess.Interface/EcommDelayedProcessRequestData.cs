using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommDelayedProcess.Interface
{
  public class EcommDelayedProcessRequestData : RequestData
  {
    public string EncryptedResult { get; set; }
    public string InvoiceID { get; set; }

    public EcommDelayedProcessRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,string encryptedResult,string invoiceID)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(40);
      EncryptedResult = encryptedResult;
      InvoiceID = invoiceID;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EcommDelayedProcessRequestData");     
    }


  }
}
