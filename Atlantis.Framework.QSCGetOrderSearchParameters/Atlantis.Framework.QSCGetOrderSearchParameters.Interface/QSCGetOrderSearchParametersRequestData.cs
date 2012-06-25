using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCGetOrderSearchParameters.Interface
{
  public class QSCGetOrderSearchParametersRequestData : RequestData
  {
    public QSCGetOrderSearchParametersRequestData(string shopperId, 
                                                  string sourceURL, 
                                                  string orderId, 
                                                  string pathway, 
                                                  int pageCount) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
    }

    private string CacheKey
    {
      get { return "QSCOrderSearchParameterList"; }
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
