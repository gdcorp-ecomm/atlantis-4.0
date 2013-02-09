﻿
using System;
using System.Web;
using System.Xml.Linq;
using Atlantis.Framework.PixelsGet.Interface.PixelObjects.Helpers;
using Atlantis.Framework.PixelsGet.Interface.Constants;
using Atlantis.Framework.BasePages.Cookies;


namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.Triggers
{
  public class CookieGroupTrigger : Trigger
  {
    public CookieGroupTrigger(XElement triggerElement, PixelsGetRequestData pixelRequest)
      : base(triggerElement, pixelRequest)
    { }

    public override string TriggerType()
    {
      return PixelXmlNames.TriggerTypeCookieGroup;
    }

    public override bool ShouldFirePixel(bool isPixelAlreadyTriggered)
    {
      bool shouldFirePixel = false;
      if (ContinuePixelFireCheck)
      {
        foreach (XElement element in TriggerElement.Descendants(PixelXmlNames.TriggerTypeCookieGroup))
        {
          string fireCookieValue = element.Attribute(PixelXmlNames.Value).Value;
          string cookieName = element.Attribute(PixelXmlNames.TriggerCookieName).Value;
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
              if (element.Attribute(PixelXmlNames.CookieEncoded) != null)
              {
                shouldFirePixel = Atlantis.Framework.BasePages.Cookies.CookieHelper.DecryptCookieValue(searchCookie.Value).Equals(fireCookieValue, StringComparison.OrdinalIgnoreCase);
              }
              else
              {
                shouldFirePixel = searchCookie.Value.Equals(fireCookieValue, StringComparison.OrdinalIgnoreCase);
              }
            }
            else
            {
              shouldFirePixel = true;
            }
          }

          if (shouldFirePixel)
          {
            RemoveAllCookiesIfNeccesary(TriggerElement, associatedCookieNames);
          }
        }
      }

      return shouldFirePixel;
    }

    
    private void RemoveAllCookiesIfNeccesary(XElement element, string associatedCookieNames)
    {
      var attribute = element.Attribute(PixelXmlNames.RemoveAfterConsumption);
      if (attribute != null)
      {
        if (attribute.Value.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
          if (PixelRequest.RequestCookies != null)
          {
            foreach (string associatedCookieName in associatedCookieNames.Split(','))
            {
              HttpCookie tempCookie = PixelRequest.RequestCookies[associatedCookieName];
              if (tempCookie != null)
              {
                PixelRequest.RequestCookies.Remove(associatedCookieName);
                if (HttpContext.Current != null)
                {
                  HttpContext.Current.Response.Cookies.Remove(associatedCookieName);
                  PixelObjects.Helpers.CookieHelper cookieHelper = new PixelObjects.Helpers.CookieHelper();
                  HttpCookie cookie = cookieHelper.NewCrossDomainCookie(associatedCookieName, DateTime.Now.AddDays(-1));
                  HttpContext.Current.Response.Cookies.Add(cookie);
                }
              }
            }
          }
        }
      }
    }
  }
}
