using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BazaarVoiceAPI.Interface
{
  public class ReviewsAPIRequestData : RequestData
  {
    private NameValueCollection _parameters = new NameValueCollection();

    public ReviewsAPIRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
    }

    public void AddQueryParameter(string key, string value)
    {
      _parameters[key] = value;
    }

    public void AddQueryParameters(NameValueCollection queryItems)
    {
      if (queryItems != null)
      {
        foreach (string key in queryItems.Keys)
        {
          _parameters[key] = queryItems[key];
        }
      }
    }

    public string GetQuery()
    {
      List<string> queryItems = new List<string>(_parameters.Count);

      foreach (string key in _parameters.Keys)
      {
        if ((!string.IsNullOrEmpty(key)) && (_parameters[key] != null))
        {
          string queryItem = Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(_parameters[key]);
          queryItems.Add(queryItem);
        }
      }

      // To allow for consistent keys, sort the parameters
      queryItems.Sort();
      var queryString = string.Join("&", queryItems);
      return queryString;
    }

    public override string GetCacheMD5()
    {
      using (MD5 oMD5 = new MD5CryptoServiceProvider())
      {
        oMD5.Initialize();
        byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(GetQuery());
        byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
        string sValue = BitConverter.ToString(md5Bytes, 0);
        return sValue.Replace("-", string.Empty);
      }
    }
  }
}
