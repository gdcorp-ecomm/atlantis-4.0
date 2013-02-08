
using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;
using Atlantis.Framework.PixelsGet.Interface.PixelObjects.Helpers;
using Atlantis.Framework.PixelsGet.Interface.Constants;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.Triggers
{
  public class CookieTrigger : Trigger
  {
    private List<string> _allCookieNames = new List<string>();

    public CookieTrigger(XElement triggerElement, PixelsGetRequestData pixelRequest)
      : base(triggerElement, pixelRequest)
    { }

    public override string TriggerType()
    {
      return PixelXmlNames.TriggerTypeCookie;
    }

    public override bool ShouldFirePixel(bool isPixelAlreadyTriggered)
    {
      bool shouldFirePixel = false;
      if (ContinuePixelFireCheck)
      {
        foreach (XElement element in TriggerElement.Descendants(PixelXmlNames.TriggerTypeCookie))
        {
          string fireCookieValue = element.Attribute(PixelXmlNames.Value).Value;
          if (element.Attribute(PixelXmlNames.CookieEncoded) != null)
          {
            fireCookieValue = Atlantis.Framework.BasePages.Cookies.CookieHelper.DecryptCookieValue(fireCookieValue);
          }
          string cookieName = element.Attribute(PixelXmlNames.Name).Value;

          if (TagReplacer.ReplaceTagOnElement(TriggerElement))
          {
            TagReplacer replacer = new TagReplacer(PixelRequest.ReplaceTags);
            fireCookieValue = replacer.ReplaceTagsIn(fireCookieValue);
            cookieName = replacer.ReplaceTagsIn(cookieName);
          }

          _allCookieNames.Add(cookieName);

          if (!shouldFirePixel && !isPixelAlreadyTriggered)
          {
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
          }
        }

        if (shouldFirePixel || isPixelAlreadyTriggered)
        {
          RemoveAllCookiesIfNeccesary(TriggerElement);
        }
      }
      
      return shouldFirePixel;
    }

    private void RemoveAllCookiesIfNeccesary(XElement element)
    {
      var attribute = element.Attribute(PixelXmlNames.RemoveAfterConsumption);
      if (attribute != null)
      {
        if (attribute.Value.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
          foreach (string cookieName in _allCookieNames)
          {
            RemoveCookie(cookieName);
          } 
        }
      }
    }

    private void RemoveCookie(string cookieName)
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
