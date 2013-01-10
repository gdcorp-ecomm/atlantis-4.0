using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.PixelsGet.Interface.PixelObjects.Triggers.GlobalTriggerChecks;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.Triggers
{
  public abstract class Trigger
  {
    #region Private Properties

    /// <summary>
    /// Add all the attributes to this list you want to check before the custom triggers check.  
    /// </summary>

    private List<GlobalTriggerCheck> _globalAttributesToCheck;
    private List<GlobalTriggerCheck> GlobalAttributesToCheck
    {
      get
      {
        if (_globalAttributesToCheck == null)
        {
          _globalAttributesToCheck = new List<GlobalTriggerCheck>();
          _globalAttributesToCheck.Add(new FirstTimeOnlyShopperCheck(PixelRequest.FirstTimeShopper)); 
        }

        return _globalAttributesToCheck;
      }
    }
    #endregion
    #region Public properties
    public XElement TriggerElement { get; set; }
    public bool ContinuePixelFireCheck { get; set; }

    public PixelsGetRequestData PixelRequest { get; set; }
    #endregion

    protected Trigger(XElement triggerElement, PixelsGetRequestData pixelRequest)
    {
      TriggerElement = triggerElement;
      PixelRequest = pixelRequest;

      CheckGlobalAttributes();
    }

    private void CheckGlobalAttributes()
    {
      bool ceaseFireChecks = false;

      foreach (GlobalTriggerCheck triggerCheck in GlobalAttributesToCheck)
      {
        XAttribute globalAttribute = TriggerElement.Attribute(triggerCheck.PixelXmlName);

        if (globalAttribute != null)
        {
          if (triggerCheck.CeaseFiringTrigger(globalAttribute.Value))
          {
            ceaseFireChecks = true;
            break;
          }
        }
      }

      ContinuePixelFireCheck = !ceaseFireChecks;
    }

    //TODO: Figure out how to have a dynamic amount of reference params to avoid needing to repro this logic in all triggers.
    //METHOD NOT CURRENTLY USED
    public void ReplaceTags(XElement element, ref string[] allTriggerParms)
    {
      if (TagReplacer.ReplaceTagOnElement(element))
      {
        TagReplacer replacer = new TagReplacer(PixelRequest.ReplaceTags);
        foreach (string replaceParm in allTriggerParms)
        {
          if (replaceParm.Length > 0)
          {
            replacer.ReplaceTagsIn(replaceParm);
          }
        }
      }
    }

    public virtual bool ShouldFirePixel()
    {
      throw new NotImplementedException("Must implement custom logic");
    }

    public virtual bool ShouldFirePixel(bool pixelAlreadyTriggering)
    {
      throw new NotImplementedException("Must implement custom logic");
    }
  }
}
