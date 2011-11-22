using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMGetQuotaAndPermissions.Interface
{
  public class EEMGetQuotaAndPermissionsRequestData : RequestData
  {
    #region Properites
    private int _requestType = 454;
    public int EEMQuotaAndPermissionsRequestType
    {
      get { return _requestType; }
      set { _requestType = value; }
    }
    public int Pfid { get; private set; }
    #endregion


    public EEMGetQuotaAndPermissionsRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int pfid)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)    
    {
      Pfid = pfid;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(string.Format("{0}", Pfid));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
