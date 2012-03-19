using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CDSRepository.Interface
{

  public class CDSRepositoryRequestData : RequestData
  {

    public CDSRepositoryRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  string query)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      Query = query;
      RequestTimeout = TimeSpan.FromSeconds(20);
    }

    public string Query { get; set; }
    //public TimeSpan RequestTimeout { get; set; }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes("CDSRepositoryRequestData:Query:" + Query);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }

  }
}
