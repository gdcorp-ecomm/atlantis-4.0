using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WSTService.Interface
{
  public class WSTTemplateInfoRequestData : RequestData
  {
    public int CategoryID { get; private set; }

    public WSTTemplateInfoRequestData(string sShopperID,
                                      string sSourceURL,
                                      string sOrderID,
                                      string sPathway,
                                      int iPageCount)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      CategoryID = 0;
      RequestTimeout = new TimeSpan(0, 0, 5);
    }

    public WSTTemplateInfoRequestData(int categoryID,
                                      string sShopperID,
                                      string sSourceURL,
                                      string sOrderID,
                                      string sPathway,
                                      int iPageCount)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      CategoryID = categoryID;
      RequestTimeout = new TimeSpan(0, 0, 5);
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(GetCacheKey());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }

    private string GetCacheKey()
    {
      return string.Format("WSTTemplateInfoRequestData_CategoryID={0}", CategoryID);
    }
  }
}
