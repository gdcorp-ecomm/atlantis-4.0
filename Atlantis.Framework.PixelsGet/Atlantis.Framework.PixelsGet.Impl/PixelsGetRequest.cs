using System;
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
        XDocument.Load(pathToLoad, LoadOptions.None);
      }

      foreach (XNode node in pixelData.Root.Nodes())
      {
        try
        {
          XElement currentElement = (XElement)node;
          if (currentElement != null && currentElement.Attribute(PixelXmlNames.AppName).Value == _pixelRequestData.AppName)
          {
            if (TriggerPixel(currentElement))
            {
              Pixel newPixel = CreatePixel(currentElement);
              pixels.Add(newPixel);
            }
          }
        }
        catch (Exception ex)
        {
          var aex = new AtlantisException(_pixelRequestData, "PixelGetRequest::GetAppPixelsForApp",
                                          "Pixel get failed.  Potentially invalid xml in pixel file", ex.StackTrace);
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

      Pixel newPixel = new Pixel(value, name, pixelType, ciCodes, appSettingName);
      return newPixel;
    }

    private bool TriggerPixel(XElement currentElement)
    {
      bool shouldTriggerPixel = false;

      if (IsValidContext(currentElement))
      {
        string validPageUrl = currentElement.Attribute(PixelXmlNames.PageUrl).Value.ToLower();

        if (SourcePageMatchesPixelUrl(validPageUrl))
        {
          Trigger currentTrigger = GetTrigger(currentElement);
          if (currentTrigger != null)
          {
            shouldTriggerPixel = currentTrigger.ShouldFirePixel();
          }
        }
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
    private Trigger GetTrigger(XElement currentElement)
    {
      Trigger trigger = null;
      Type triggerType = null;

      XElement triggerElement = currentElement.Descendants(PixelXmlNames.Trigger).First(x => x != null);
      XElement triggerTypeElement = (XElement)triggerElement.Nodes().First(x => x != null);
      string triggerTypeName = triggerTypeElement.Name.ToString();

      switch (triggerTypeName)
      {
        case PixelXmlNames.TriggerTypeCookie:
          triggerType = typeof(CookieTrigger);
          break;
        case PixelXmlNames.TriggerTypeIscCodes:
          triggerType = typeof(SourceCodeTrigger);
          break;
        case PixelXmlNames.TriggerTypeCookieGroup:
          triggerType = typeof (CookieGroupTrigger);
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
