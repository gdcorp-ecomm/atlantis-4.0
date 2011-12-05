using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSGetResourceById.Interface
{
  public class DBSGetResourceByIdRequestData : RequestData
  {
    private int _resourceId;

    public DBSGetResourceByIdRequestData( 
      string shopperId, string sourceURL, string orderId, string pathway, int pageCount, 
      int resourceId)
      : base (shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _resourceId = resourceId;
      RequestTimeout = TimeSpan.FromSeconds(4);
    }

    public int ResourceId
    {
      get { return _resourceId; }
      set { _resourceId = value; }
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(_resourceId.ToString());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}
