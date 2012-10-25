using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WSTService.Interface
{
  public class WSTTemplateInfoRequestData : RequestData
  {
    public int CategoryId { get; private set; }

    public WSTTemplateInfoRequestData(string shopperId,
                                      string sourceURL,
                                      string orderId,
                                      string pathway,
                                      int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      CategoryId = 0;
      RequestTimeout = new TimeSpan(0, 0, 5);
    }

    public WSTTemplateInfoRequestData(int categoryId,
                                      string shopperId,
                                      string sourceURL,
                                      string orderId,
                                      string pathway,
                                      int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      CategoryId = categoryId;
      RequestTimeout = new TimeSpan(0, 0, 5);
    }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();
      md5.Initialize();
      byte[] stringBytes = ASCIIEncoding.ASCII.GetBytes(GetCacheKey());
      byte[] md5Bytes = md5.ComputeHash(stringBytes);
      string value = BitConverter.ToString(md5Bytes, 0);
      return value.Replace("-", string.Empty);
    }

    private string GetCacheKey()
    {
      return string.Format("WSTTemplateInfoRequestData_CategoryId={0}", CategoryId);
    }
  }
}
