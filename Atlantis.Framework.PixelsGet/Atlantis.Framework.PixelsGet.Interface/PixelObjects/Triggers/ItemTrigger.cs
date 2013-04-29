
using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.PixelsGet.Interface.Constants;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.Triggers
{
  public class ItemTrigger : Trigger
  {
    public ItemTrigger(XElement triggerElement, PixelsGetRequestData pixelRequest)
      : base(triggerElement, pixelRequest)
    { }

    public override string TriggerType()
    {
      return PixelXmlNames.TriggerTypeItemSingle;
    }

    public override bool ShouldFirePixel(bool isPixelAlreadyTriggered, List<Pixel> alreadyFiredPixels, ref string triggerReturn)
    {
      bool shouldFirePixel = false;
      if (ContinuePixelFireCheck && !isPixelAlreadyTriggered)
      {
        if (!string.IsNullOrEmpty(PixelRequest.OrderXml))
        {
          XmlDocument _orderXMLDoc = new XmlDocument();
          _orderXMLDoc.Load(new System.IO.StringReader(PixelRequest.OrderXml));

          XmlNode _orderItems = _orderXMLDoc.SelectSingleNode("//ITEMS");

          foreach (XElement element in TriggerElement.Descendants(PixelXmlNames.TriggerTypeItemSingle))
          {
            string fireOrderItemName = element.Attribute(PixelXmlNames.Name).Value;
            string fireOrderItemValue = element.Attribute(PixelXmlNames.Value).Value;

            foreach (XmlNode orderItem in _orderItems.ChildNodes)
            {
              if (orderItem.Attributes != null)
              {
                if (orderItem.Attributes[fireOrderItemName] != null)
                {
                  string orderItemValue = orderItem.Attributes[fireOrderItemName].Value;
                  if (orderItemValue.Equals(fireOrderItemValue, StringComparison.OrdinalIgnoreCase))
                  {
                    shouldFirePixel = true;
                    break;
                  }
                }
              }
            }

            if (shouldFirePixel)
            {
              break;
            }
          }
        }
      }

      return shouldFirePixel;
    }
  }
}
