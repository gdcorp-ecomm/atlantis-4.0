
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

    public override bool ShouldFirePixel(bool isPixelAlreadyTriggered, List<Pixel> alreadyFiredPixels, ref string triggerReturn)
    {

      bool returnValue = false;

      if (ContinuePixelFireCheck)
      {
        foreach (XElement element in TriggerElement.Descendants(PixelXmlNames.TriggerTypeCookie))
        {
          string elCookieValue = element.Attribute(PixelXmlNames.Value).Value;
          if (element.Attribute(PixelXmlNames.CookieEncoded) != null)
          {
            elCookieValue = Atlantis.Framework.BasePages.Cookies.CookieHelper.DecryptCookieValue(elCookieValue);
          }
          string elCookieName = element.Attribute(PixelXmlNames.Name).Value;

          if (TagReplacer.ReplaceTagOnElement(TriggerElement))
          {
            TagReplacer replacer = new TagReplacer(PixelRequest.ReplaceTags);
            elCookieValue = replacer.ReplaceTagsIn(elCookieValue);
            elCookieName = replacer.ReplaceTagsIn(elCookieName);
          }

          _allCookieNames.Add(elCookieName);

          if (!returnValue && !isPixelAlreadyTriggered)
          {
            HttpCookie existingCookie = PixelRequest.RequestCookies[elCookieName];

            // Fire pixel when...
            // Default: Cookie exists (fireWhen element is missing)
            // fireWhen element = always
            // Cookie exists and fireWhen element = present
            // Cookie missing and fireWhen element = missing

            XAttribute fireCondition = element.Attribute("fireWhen");
            // Default is fire if the cookie exists
            bool firePixel = !ReferenceEquals(null, existingCookie);
            bool checkValue = firePixel;
            if (!ReferenceEquals(null, fireCondition))
            {
              firePixel = string.Equals("always", fireCondition.Value, StringComparison.OrdinalIgnoreCase);
              checkValue = false;
              // the fireWhen elment exists and doesn't equal "always"
              if (!firePixel)
              {
                // Cookie missing and fireWhen element = missing
                firePixel = string.Equals("missing", fireCondition.Value, StringComparison.OrdinalIgnoreCase) && ReferenceEquals(null, existingCookie);

                if (!firePixel)
                {
                  // Cookie exists and fireWhen element = present
                  firePixel = string.Equals("present", fireCondition.Value, StringComparison.OrdinalIgnoreCase) && !ReferenceEquals(null, existingCookie);
                  checkValue = firePixel;
                }
              }
              returnValue = firePixel;
            }

            if (firePixel && checkValue)
            {
              bool isFireCookieValueEmpty = string.IsNullOrEmpty(elCookieValue); //essentially using empty value as a wildcard. As long as cookie exists, fire!
              if (!isFireCookieValueEmpty)
              {
                if (element.Attribute(PixelXmlNames.CookieEncoded) != null)
                {
                  returnValue = string.Equals(elCookieValue, Atlantis.Framework.BasePages.Cookies.CookieHelper.DecryptCookieValue(existingCookie.Value), StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                  returnValue = string.Equals(elCookieValue, existingCookie.Value, StringComparison.OrdinalIgnoreCase);
                }
              }
              else
              {
                returnValue = true;
              }
            }
          }
        }

        if (returnValue || isPixelAlreadyTriggered)
        {
          RemoveAllCookiesIfNeccesary(TriggerElement);
        }
      }

      return returnValue;
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
