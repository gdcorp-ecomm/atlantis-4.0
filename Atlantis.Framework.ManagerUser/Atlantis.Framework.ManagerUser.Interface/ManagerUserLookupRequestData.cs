using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using System.Security.Cryptography;

namespace Atlantis.Framework.ManagerUser.Interface
{
  public class ManagerUserLookupRequestData : RequestData
  {
    private string _domain;
    private string _userId;

    public ManagerUserLookupRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      string domain, string userId) 
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _domain = domain;
      _userId = userId;
    }

    public string Domain
    {
      get { return _domain; }
    }

    public string UserId
    {
      get { return _userId; }
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes
        = System.Text.ASCIIEncoding.ASCII.GetBytes(_domain.ToString() + @"\" + _userId.ToString());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
