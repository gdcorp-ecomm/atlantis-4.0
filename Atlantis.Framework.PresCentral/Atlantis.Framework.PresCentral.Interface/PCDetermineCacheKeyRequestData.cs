using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresCentral.Interface
{
  public class PCDetermineCacheKeyRequestData : PCRequestDataBase
  {
    public PCDetermineCacheKeyRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    { }

    public override IResponseData CreateResponse(PCResponse responseData)
    {
      return new PCDetermineCacheKeyResponseData(responseData);
    }

    public override IResponseData CreateResponse(AtlantisException ex)
    {
      return new PCDetermineCacheKeyResponseData(ex);
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(GetQuery());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}
