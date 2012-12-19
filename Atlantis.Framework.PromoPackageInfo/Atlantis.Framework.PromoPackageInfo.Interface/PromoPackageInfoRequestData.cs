using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoPackageInfo.Interface
{
  public class PromoPackageInfoRequestData : RequestData
  {
    public PromoPackageInfoRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  int packageId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PackageId = packageId;
    }

    public int PackageId { get; private set; }

    #region Overridden Methods

    public override string ToXML()
    {
      return string.Empty;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(PackageId.ToString());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }

    #endregion
  }
}