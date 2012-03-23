using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Linq;

namespace Atlantis.Framework.CDS.Interface
{

  public class CDSRequestData : RequestData
  {

    public CDSRequestData(string shopperId,
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

    public CDSRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string query, string docId, DateTime activeDate)
      : this(shopperId, sourceUrl, orderId, pathway, pageCount, query)
    {
      NameValueCollection nvc = new NameValueCollection(2);
      if (activeDate != default(DateTime))
      {
        nvc.Add("activedate", activeDate.ToString("O"));
      }
      if (IsValidMongoObjectId(docId))
      {
        nvc.Add("docid", docId);
      }
      if (nvc.Count > 0)
      {
        string appendChar = Query.Contains("?") ? "&" : "?";
        Query += string.Concat(appendChar, ToQueryString(nvc));
      }
    }

    public string Query { get; private set; }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes("CDSRequestData:Query:" + Query);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }

    private bool IsValidMongoObjectId(string text)
    {
      bool result = false;
      if (text != null)
      {
        string pattern = @"^[0-9a-fA-F]{24}$";
        result = Regex.IsMatch(text, pattern);
      }
      return result;
    }

    private string ToQueryString(NameValueCollection nvc)
    {
      return string.Join("&", nvc.AllKeys.SelectMany(key => nvc.GetValues(key).Select(value => string.Format("{0}={1}", key, value))).ToArray());
    }
  }
}
