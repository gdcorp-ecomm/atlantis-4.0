using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCThemeImages.Interface
{
  public class QSCThemeImagesRequestData : RequestData
  {

    public QSCThemeImagesRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(10);

    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("QSCImages:{0}", ShopperID));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }


  }
}
