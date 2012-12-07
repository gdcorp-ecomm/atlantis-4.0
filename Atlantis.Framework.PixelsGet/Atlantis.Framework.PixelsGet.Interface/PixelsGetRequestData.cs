
using System;
using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PixelsGet.Interface
{
  public class PixelsGetRequestData: RequestData
  {
    #region Public properties
    public string AppName { get; set; }
    public string IscCode { get; set; }
    public HttpCookieCollection RequestCookies { get; set; }
    public string OrderXml { get; set; }
    public int ContextId { get; set; }
    public Dictionary<string, string> ReplaceTags { get; set; }
    public bool FirstTimeShopper { get; set; }
    public string XmlFilePathOverride { get; set; }
    #endregion

    public PixelsGetRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
                                     string appName, string iscCode, HttpCookieCollection requestCookies, Dictionary<string, string> replaceTags,
                                     int contextId,  string orderXml = "")
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      AppName = appName;
      IscCode = iscCode;
      RequestCookies = requestCookies;
      ReplaceTags = replaceTags;
      ContextId = contextId;
      OrderXml = orderXml;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public PixelsGetRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
                                 string appName, string iscCode, HttpCookieCollection requestCookies, Dictionary<string, string> replaceTags,
                                 int contextId,  bool firstTimeShopper, string orderXml = "")
      : this(shopperId, sourceUrl, orderId, pathway, pageCount, appName, iscCode, requestCookies, replaceTags, contextId, orderXml)
    {
      FirstTimeShopper = firstTimeShopper;
    }
    
    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
