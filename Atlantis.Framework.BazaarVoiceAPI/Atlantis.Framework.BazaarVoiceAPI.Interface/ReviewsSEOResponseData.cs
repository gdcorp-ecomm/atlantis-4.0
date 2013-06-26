using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.BazaarVoiceAPI.Interface
{
  public class ReviewsSEOResponseData : IResponseData
  {

    public string HTML { get; private set; }
    
    public ReviewsSEOResponseData(string html, string uri)
    {
      HTML = html;
      URI = uri;

    }

    public string GetHtmlWithReplacementUri(string CurrentPageUri)
    {
      string separator = CurrentPageUri.Contains("?") ? "&" : "?";
      return HTML.Replace("{INSERT_PAGE_URI}", string.Format("{0}{1}", CurrentPageUri, separator));    
    }

    public string URI { get; private set; }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
