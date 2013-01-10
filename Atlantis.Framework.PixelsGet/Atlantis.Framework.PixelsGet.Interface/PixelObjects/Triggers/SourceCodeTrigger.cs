
using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;
using Atlantis.Framework.PixelsGet.Interface.Constants;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.Triggers
{
  public class SourceCodeTrigger : Trigger
  {
    public SourceCodeTrigger(XElement triggerElement, PixelsGetRequestData pixelRequest)
      : base(triggerElement, pixelRequest)
    { }

    public override bool ShouldFirePixel(bool isPixelAlreadyTriggered)
    {
      bool shouldFirePixel = false;
      if (ContinuePixelFireCheck)
      {
        foreach (XElement element in TriggerElement.Descendants(PixelXmlNames.TriggerTypeIscCodeSingle))
        {
          string fireSourceCodeValue = element.Attribute("value").Value;
          bool isPrefixOnlyIsc = fireSourceCodeValue.EndsWith("*");

          if (!isPrefixOnlyIsc)
          {
            shouldFirePixel = PixelRequest.IscCode.Equals(fireSourceCodeValue, StringComparison.OrdinalIgnoreCase);
          }
          else
          {
            string prefix = fireSourceCodeValue.Split('*')[0];
            shouldFirePixel = PixelRequest.IscCode.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
          }

          if (shouldFirePixel)
          {
            break;
          }
        }
      }

      return shouldFirePixel;
    }
  }
}
