
using System;
using System.Web;
using System.Xml.Linq;
using Atlantis.Framework.PixelsGet.Interface.PixelObjects.Helpers;
using Atlantis.Framework.PixelsGet.Interface.Constants;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.Triggers
{
  public class CookieTrigger : Trigger
  {
    public CookieTrigger(XElement triggerElement, PixelsGetRequestData pixelRequest)
      : base(triggerElement, pixelRequest)
    { }

    public override bool ShouldFirePixel()
    {
      bool shouldFirePixel = false;
      if (ContinuePixelFireCheck)
      {
        foreach (XElement element in TriggerElement.Descendants(PixelXmlNames.TriggerTypeCookie))
        {
          string fireCookieValue = element.Attribute(PixelXmlNames.Value).Value;
          string cookieName = element.Attribute(PixelXmlNames.Name).Value;

          if (TagReplacer.ReplaceTagOnElement(TriggerElement))
          {
            TagReplacer replacer = new TagReplacer(PixelRequest.ReplaceTags);
            fireCookieValue = replacer.ReplaceTagsIn(fireCookieValue);
            cookieName = replacer.ReplaceTagsIn(cookieName);
          }

          HttpCookie searchCookie = PixelRequest.RequestCookies[cookieName];

          if (searchCookie != null)
          {
            bool isFireCookieValueEmpty = string.IsNullOrEmpty(fireCookieValue); //essentially using empty value as a wildcard. As long as cookie exists, fire!

            if (!isFireCookieValueEmpty)
            {
              shouldFirePixel = searchCookie.Value.Equals(fireCookieValue, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
              shouldFirePixel = true;
            }
          }

          if (shouldFirePixel)
          {
            RemoveCookieIfNeccesary(TriggerElement, cookieName);
            break;
          }
        }
      }

      return shouldFirePixel;
    }

    private void RemoveCookieIfNeccesary(XElement element, string cookieName)
    {
      var attribute = element.Attribute(PixelXmlNames.RemoveAfterConsumption);
      if (attribute != null)
      {
        if (attribute.Value.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
          if (HttpContext.Current != null)
          {
            HttpCookie tempCookie = HttpContext.Current.Response.Cookies[cookieName];
            if (tempCookie != null)
            {
              CookieHelper cookieHelper = new CookieHelper();
              HttpCookie cookie = cookieHelper.NewCrossDomainCookie(cookieName, DateTime.Now.AddDays(-1));
              HttpContext.Current.Response.Cookies.Remove(cookieName);
              HttpContext.Current.Response.Cookies.Add(cookie);
            }
          }
        }
      }
    }
  }
}
