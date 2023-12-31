﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PixelsGet.Interface;
using Atlantis.Framework.PixelsGet.Interface.Constants;
using Atlantis.Framework.PixelsGet.Interface.PixelObjects;
using Atlantis.Framework.PixelsGet.Interface.PixelObjects.AdditionalDataParams;
using Atlantis.Framework.PixelsGet.Interface.PixelObjects.Triggers;

namespace Atlantis.Framework.PixelsGet.Impl
{
  public class PixelsGetRequest : IRequest
  {
    private PixelsGetRequestData _pixelRequestData;
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      PixelsGetResponseData responseData = null;

      try
      {
        _pixelRequestData = (PixelsGetRequestData)requestData;
        List<Pixel> pixels = GetAllPixelsForApp();
        responseData = new PixelsGetResponseData(pixels);
      }
      catch (AtlantisException aex)
      {
        responseData = new PixelsGetResponseData(aex);
      }
      catch (Exception ex)
      {
        responseData = new PixelsGetResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Get all the pixels for the calling app

    private List<Pixel> GetAllPixelsForApp()
    {
      List<Pixel> pixels = new List<Pixel>();
      string pathToLoad = "pixels.xml";

      if (!string.IsNullOrEmpty(_pixelRequestData.XmlFilePathOverride))
      {
        pathToLoad = _pixelRequestData.XmlFilePathOverride;
      }

      XDocument pixelData = null;

      if (_pixelRequestData.XDocumentOverride != null)
      {
        pixelData = _pixelRequestData.XDocumentOverride;
      }
      else
      {
        pixelData = XDocument.Load(pathToLoad, LoadOptions.None);
      }

      foreach (XNode node in pixelData.Root.Nodes())
      {
        try
        {
          XElement currentElement = (XElement)node;
          if (currentElement != null && currentElement.Attribute(PixelXmlNames.AppName).Value == _pixelRequestData.AppName)
          {
            string triggerReturn = String.Empty;
            if (TriggerPixel(currentElement, pixels, ref triggerReturn))
            {
              Pixel newPixel = CreatePixel(currentElement);
              newPixel.TriggerReturn = triggerReturn;
              pixels.Add(newPixel);
            }
          }
        }
        catch (Exception ex)
        {
          var aex = new AtlantisException(_pixelRequestData, "PixelGetRequest::GetAppPixelsForApp",
                                          "Pixel get failed.  Potentially invalid xml in pixel file", ex.StackTrace + pixelData.ToString());
          Engine.Engine.LogAtlantisException(aex);
        }
      }

      return pixels;
    }

    private Pixel CreatePixel(XElement currentElement)
    {
      IEnumerable<XElement> additionalData = currentElement.Descendants(PixelXmlNames.AdditionalData);

      string value = currentElement.Attribute(PixelXmlNames.Value).Value;
      string pixelType = currentElement.Attribute(PixelXmlNames.PixelType).Value;
      string name = currentElement.Attribute(PixelXmlNames.Name).Value;
      string appSettingName = string.Empty;
      List<string> ciCodes = new FastballParam().GetListData(additionalData);

      XAttribute appSetting = currentElement.Attribute(PixelXmlNames.AppSetting);
      if (appSetting != null)
      {
        appSettingName = appSetting.Value;
      }


      if (TagReplacer.ReplaceTagOnElement(currentElement))
      {
        TagReplacer replacer = new TagReplacer(_pixelRequestData.ReplaceTags);
        value = replacer.ReplaceTagsIn(value);
      }

      IList<CookieData> cookieValues = new List<CookieData>();
      var cookieValuesXml = currentElement.Descendants(PixelXmlNames.TriggerTypeCookie);
      if (!ReferenceEquals(null, cookieValuesXml))
      {
        foreach (var item in cookieValuesXml)
        {
          string cookieName = ReferenceEquals(null, item.Attribute(PixelXmlNames.Name)) ? string.Empty : item.Attribute(PixelXmlNames.Name).Value;
          string cookieValue = ReferenceEquals(null, item.Attribute(PixelXmlNames.Value)) ? string.Empty : item.Attribute(PixelXmlNames.Value).Value;
          int expirationDays = ReferenceEquals(null, item.Attribute(PixelXmlNames.ExpirationDays)) ? 30 : int.Parse(item.Attribute(PixelXmlNames.ExpirationDays).Value);
          bool isEncoded = ReferenceEquals(null, item.Attribute(PixelXmlNames.CookieEncoded)) ? false : bool.Parse(item.Attribute(PixelXmlNames.CookieEncoded).Value);
          if (!ReferenceEquals(null, _pixelRequestData.ReplaceTags) && TagReplacer.ReplaceTagOnElement(item))
          {
            TagReplacer replacer = new TagReplacer(_pixelRequestData.ReplaceTags);
            cookieName = replacer.ReplaceTagsIn(cookieName);
            cookieValue = replacer.ReplaceTagsIn(cookieValue);
          }
          cookieValues.Add(new CookieData(cookieName, expirationDays, isEncoded, cookieValue));
        }
      }

      Pixel newPixel = new Pixel(value, name, pixelType, ciCodes, cookieValues, appSettingName);
      return newPixel;
    }

    private bool TriggerPixel(XElement currentElement, List<Pixel> pixelsFired, ref string triggerReturn)
    {
      bool shouldTriggerPixel = false;
      try
      {

        if (IsValidContext(currentElement))
        {
          if (currentElement.Attribute(PixelXmlNames.PageUrl) != null)
          {
            string validPageUrl = currentElement.Attribute(PixelXmlNames.PageUrl).Value.ToLower();

            if (SourcePageMatchesPixelUrl(validPageUrl))
            {
              if (currentElement.Descendants(PixelXmlNames.Trigger).Count() == 0)
              {
                shouldTriggerPixel = true;
              }
              else
              {
                XElement triggerElement = currentElement.Descendants(PixelXmlNames.Trigger).First(x => x != null);
                bool pixelAlreadyTriggered = false;
                string[] requiredTriggers = { "" };
                if (triggerElement.Attribute(PixelXmlNames.Required) != null)
                {
                  requiredTriggers = triggerElement.Attribute(PixelXmlNames.Required).Value.ToLower().Split(',');
                  foreach (string requiredTrigger in requiredTriggers)
                  {
                    if (triggerElement.Descendants(requiredTrigger).Count() == 0)
                    {
                      var aex = new AtlantisException(_pixelRequestData, "PixelGetRequest::TriggerPixel",
                                                      "Missing required trigger " + requiredTrigger + " in XML", currentElement.ToString());
                      Engine.Engine.LogAtlantisException(aex);
                    }
                  }
                }
                foreach (XElement individualTrigger in triggerElement.Nodes())
                {
                  Trigger currentTrigger = GetTrigger(triggerElement, individualTrigger);
                  if (currentTrigger != null)
                  {
                    if (requiredTriggers.Contains(currentTrigger.TriggerType()))
                    {
                      shouldTriggerPixel = currentTrigger.ShouldFirePixel(pixelAlreadyTriggered, pixelsFired, ref triggerReturn);
                      if (!shouldTriggerPixel)
                      {
                        break;
                      }
                    }
                    else if (!shouldTriggerPixel)
                    {
                      shouldTriggerPixel = currentTrigger.ShouldFirePixel(pixelAlreadyTriggered, pixelsFired, ref triggerReturn);
                    }
                    else
                    {
                      pixelAlreadyTriggered = true;
                      currentTrigger.ShouldFirePixel(pixelAlreadyTriggered, pixelsFired, ref triggerReturn);
                    }
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException(_pixelRequestData, "PixelGetRequest::TriggerPixel",
                                        "Pixel get failed.  Potentially invalid xml in pixel element", ex.StackTrace + " " + currentElement.ToString());
        Engine.Engine.LogAtlantisException(aex);
      }

      return shouldTriggerPixel;
    }

    #region Ensure pixel should fire for context
    private bool IsValidContext(XElement currentElement)
    {
      bool isValidContext;
      string[] contexts = currentElement.Attribute(PixelXmlNames.Contexts).Value.Split(',');

      if (!contexts.Contains("*"))  //WILDCARD.  All contexts valid if *
      {
        isValidContext = contexts.Contains(_pixelRequestData.ContextId.ToString());
      }
      else
      {
        isValidContext = true;
      }

      return isValidContext;
    }
    #endregion

    #region Does the request.url match the url assigned to the pixel
    private bool SourcePageMatchesPixelUrl(string validPageUrl)
    {
      if (!validPageUrl.Contains("/"))
      {
        validPageUrl = string.Concat("/", validPageUrl); //prefix with forward slash to compare "/mypage.asp" instead of "mypage.asp" to avoid scenarios like "thisismypage.asp" matching
      }

      bool isPageValid = _pixelRequestData.SourceURL.ToLower().Contains(validPageUrl);
      return isPageValid;
    }
    #endregion

    #region Get the current trigger
    private Trigger GetTrigger(XElement triggerElement, XElement individualTrigger)
    {
      Trigger trigger = null;
      Type triggerType = null;

      string triggerTypeName = individualTrigger.Name.ToString().ToLower();

      switch (triggerTypeName)
      {
        case PixelXmlNames.TriggerTypeCookie:
          triggerType = typeof(CookieTrigger);
          break;
        case PixelXmlNames.TriggerTypeIscCodes:
          triggerType = typeof(SourceCodeTrigger);
          break;
        case PixelXmlNames.TriggerTypeItems:
          triggerType = typeof(ItemTrigger);
          break;
        case PixelXmlNames.TriggerTypeCookieGroup:
          triggerType = typeof(CookieGroupTrigger);
          break;
        case PixelXmlNames.TriggerTypeOrderDetail:
          triggerType = typeof(OrderDetailTrigger);
          break;
      }

      if (triggerType != null)
      {
        trigger = (Trigger)Activator.CreateInstance(triggerType, triggerElement, _pixelRequestData);
      }

      return trigger;
    }
    #endregion

    #endregion
  }
}
