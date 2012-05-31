using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LegalLastModifiedDate.Interface
{
  public class LegalLastModifiedDateRequestData : RequestData
  {
    public int PrivateLabelId { get; set; }
    public string PageId { get; set; }
    public TimeSpan RequestTimeout { get; set; }

    public LegalLastModifiedDateRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      string pageId, int privateLabelId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      PageId = pageId;
      RequestTimeout = TimeSpan.FromSeconds(4);
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      string privateLabel;

      if (PrivateLabelId == 1 || PrivateLabelId == 2 || PrivateLabelId == 1387)
      {
        privateLabel = PrivateLabelId.ToString();
      }
      else
      {
        privateLabel = "Reseller";
      }
      byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(privateLabel + "-" + PageId);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
