
using System;
using System.Web;
using System.Xml.Linq;
using Atlantis.Framework.PixelsGet.Interface.PixelObjects.Helpers;
using Atlantis.Framework.PixelsGet.Interface.Constants;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.Triggers
{
  public class CookieGroupTrigger : Trigger
  {
    public CookieGroupTrigger(XElement triggerElement, PixelsGetRequestData pixelRequest)
      : base(triggerElement, pixelRequest)
    { }

    public override bool ShouldFirePixel()
    {
      bool shouldFirePixel = false;
      if (ContinuePixelFireCheck)
      {
        foreach (XElement element in TriggerElement.Descendants(PixelXmlNames.TriggerTypeCookieGroup))
        {
          string fireCookieValue = element.Attribute(PixelXmlNames.Value).Value;
          string cookieName = element.Attribute(PixelXmlNames.Name).Value;
          string associatedCookieNames = element.Attribute(PixelXmlNames.AssociatedCookieNames).Value;

          if (TagReplacer.ReplaceTagOnElement(TriggerElement))
          {
            TagReplacer replacer = new TagReplacer(PixelRequest.ReplaceTags);
            fireCookieValue = replacer.ReplaceTagsIn(fireCookieValue);
            cookieName = replacer.ReplaceTagsIn(cookieName);
            associatedCookieNames = replacer.ReplaceTagsIn(associatedCookieNames);
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
            if (IsNewestCookieInGroup(searchCookie, associatedCookieNames))
            {
              RemoveAllCookiesIfNeccesary(TriggerElement, cookieName);
              break;
            }
            else
            {
              shouldFirePixel = false;
            }
          }
        }
      }

      return shouldFirePixel;
    }

    private bool IsNewestCookieInGroup(HttpCookie triggerCookie, string associatedCookies)
    {
      bool isNewestCookie = true;
      DateTime triggerCookieExpires = triggerCookie.Expires;

      foreach (var associatedCookieName in associatedCookies.Split(','))
      {
        bool isTriggerCookie = associatedCookieName.Equals(triggerCookie.Name, StringComparison.OrdinalIgnoreCase);

        if (!isTriggerCookie)
        {
          if (HttpContext.Current != null)
          {
            HttpCookie tempCookie = PixelRequest.RequestCookies[associatedCookieName];
            if (tempCookie != null)
            {
              if (tempCookie.Expires > triggerCookieExpires)
              {
                isNewestCookie = false;
                break;
              }
            }
          }
        }
      }

      return isNewestCookie;
    }

    private void RemoveAllCookiesIfNeccesary(XElement element, string associatedCookieNames)
    {
      var attribute = element.Attribute(PixelXmlNames.RemoveAfterConsumption);
      if (attribute != null)
      {
        if (attribute.Value.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
          if (HttpContext.Current != null)
          {
            foreach (string associatedCookieName in associatedCookieNames.Split(','))
            {
              HttpCookie tempCookie = HttpContext.Current.Response.Cookies[associatedCookieName];
              if (tempCookie != null)
              {
                CookieHelper cookieHelper = new CookieHelper();
                HttpCookie cookie = cookieHelper.NewCrossDomainCookie(associatedCookieName, DateTime.Now.AddDays(-1));
                HttpContext.Current.Response.Cookies.Remove(associatedCookieName);
                HttpContext.Current.Response.Cookies.Add(cookie);
              }
            }
          }
        }
      }
    }
  }
}
