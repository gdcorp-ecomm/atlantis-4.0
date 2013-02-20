using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainContactFields.Interface
{
  public class DomainContactFieldsRequestData : RequestData
  {
    public string TldName { get; private set; }

    static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(25);

    public DomainContactFieldsRequestData(string shopperId,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount,
      string tldName)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestTimeout = _requestTimeout;
      TldName = tldName;
    }

    private string CacheKey
    {
      get { return "DomainContactFields:" + TldName; }
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      MD5 oMd5 = new MD5CryptoServiceProvider();
      oMd5.Initialize();
      byte[] stringBytes = Encoding.ASCII.GetBytes(CacheKey);
      byte[] md5Bytes = oMd5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }

    #endregion
  }
}
