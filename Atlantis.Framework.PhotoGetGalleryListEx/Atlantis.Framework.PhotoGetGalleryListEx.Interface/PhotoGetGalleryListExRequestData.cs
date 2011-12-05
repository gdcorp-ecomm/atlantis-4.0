using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PhotoGetGalleryListEx.Interface
{
  public class PhotoGetGalleryListExRequestData : RequestData
  {
    private string _domain;

    public PhotoGetGalleryListExRequestData(
      string shopperID, string sourceURL, string orderID, string pathway,
      int pageCount, string domain)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _domain = domain;
      RequestTimeout = new TimeSpan(0, 0, 4);
    }

    public string Domain
    {
      get { return _domain; }
      set { _domain = value; }
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(_domain);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}