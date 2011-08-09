using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MYAGetMobileCarriers.Interface
{
  public class MYAGetMobileCarriersRequestData : RequestData
  {
    private const string MD5KEY = "Atlantis.Framework.MYAGetMobileCarriers.Interface.MYAGetMobileCarriersRequestData";

    public MYAGetMobileCarriersRequestData(string shopperId, 
                                            string sourceURL, 
                                            string orderId, 
                                            string pathway, 
                                            int pageCount) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      var data = Encoding.UTF8.GetBytes(MD5KEY);

      var hash = md5.ComputeHash(data);
      var result = Encoding.UTF8.GetString(hash);
      return result;
    }

    #endregion
  }
}
