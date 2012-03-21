using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommLastOrderLang.Interface
{
  public class EcommLastOrderLangRequestData : RequestData
  {
    private string _cookieValue = string.Empty;

    public EcommLastOrderLangRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string languageCookieValue)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _cookieValue = languageCookieValue;
      RequestTimeout = TimeSpan.FromSeconds(5);
    
    }

    public string CookieValue
    {
      get { return _cookieValue; }
      set { _cookieValue = value; }
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EcommLastOrderLangRequestData");     
    }


  }
}
