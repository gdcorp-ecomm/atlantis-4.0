using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;
using MongoDB.Bson;

namespace Atlantis.Framework.CDSRepository.Interface
{

  public class CDSRepositoryRequestData : RequestData
  {
    public CDSRepositoryRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string query)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Query = query;
      RequestTimeout = TimeSpan.FromSeconds(20);
    }

    public CDSRepositoryRequestData(string shopperId,
      string sourceUrl,
      string orderId,
      string pathway,
      int pageCount,
      string query,
      ObjectId objectId,
      DateTime activeDate)
      : this(shopperId, sourceUrl, orderId, pathway, pageCount, query)
    {
      ObjectID = objectId;
      ActiveDate = activeDate;
    }

    public string Query { get; private set; }
    public ObjectId ObjectID { get; private set; }
    public DateTime ActiveDate { get; private set; }

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
