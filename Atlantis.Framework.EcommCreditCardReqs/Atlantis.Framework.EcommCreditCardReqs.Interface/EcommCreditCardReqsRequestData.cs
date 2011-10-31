using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommCreditCardReqs.Interface
{
  public class EcommCreditCardReqsRequestData : RequestData
  {
    public string CreditCardNumber { get; set; }

    public int PrivateLabelId { get; set; }

    public string Currency { get; set; }

    public int ProfileId { get; set; }
    
    public EcommCreditCardReqsRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      string creditCardNumber, int profileId, int privateLabelId, string currency)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(4);
      CreditCardNumber = creditCardNumber;
      ProfileId = profileId;
      PrivateLabelId = privateLabelId;
      Currency = currency;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      char delimiter = '|';
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(CreditCardNumber +  delimiter +
          ProfileId + delimiter + ShopperID + delimiter + PrivateLabelId + 
          delimiter + Currency);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }

  }
}
