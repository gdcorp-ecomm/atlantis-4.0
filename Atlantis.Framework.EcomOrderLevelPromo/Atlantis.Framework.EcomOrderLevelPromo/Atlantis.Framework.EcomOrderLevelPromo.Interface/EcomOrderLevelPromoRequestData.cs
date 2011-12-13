using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcomOrderLevelPromo.Interface
{
  public class EcomOrderLevelPromoRequestData : RequestData
  {

    public EcomOrderLevelPromoRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  int privateLabelID)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      PrivateLabelID = privateLabelID;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public int PrivateLabelID{get;set;}

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();

      oMD5.Initialize();

      byte[] stringBytes

      = System.Text.ASCIIEncoding.ASCII.GetBytes("EcomOrderLevelPromo:PrivateLabelID:" + PrivateLabelID);

      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);

      string sValue = BitConverter.ToString(md5Bytes, 0);

      return sValue.Replace("-", "");
    }


  }
}
