using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperSegment.Interface
{
  public class ShopperSegmentRequestData : RequestData
  {

    public ShopperSegmentRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    { }

    
    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();
      md5.Initialize();

      byte[] md5Bytes = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(ShopperID));
      return BitConverter.ToString(md5Bytes).Replace("-", "");
    }


  }
}
