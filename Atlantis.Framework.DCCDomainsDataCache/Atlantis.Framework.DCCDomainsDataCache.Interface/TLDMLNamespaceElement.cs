using Atlantis.Framework.Interface;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public abstract class TLDMLNamespaceElement
  {
    private XElement _namespaceRootElement;

    protected XElement NamespaceElement
    {
      get { return _namespaceRootElement; }
    }

    protected abstract string Namespace { get; }
    protected abstract string LocalName { get; }

    internal TLDMLNamespaceElement(XDocument tldmlDocument)
    {
      SetNamespaceElement(tldmlDocument);
    }

    private void SetNamespaceElement(XDocument tldmlDocument)
    {
      XName searchKey = XName.Get(this.LocalName, this.Namespace);
      _namespaceRootElement = tldmlDocument.Descendants(searchKey).FirstOrDefault();
      if (_namespaceRootElement == null)
      {
        string message = this.Namespace + ":" + this.LocalName + " element not found.";
        throw new ArgumentException(message);
      }
    }

    protected int GetExpectedPeriodValue(string elementName, string expectedUnit, int defaultValueIfNotValid)
    {
      int result = defaultValueIfNotValid;

      XElement periodElement = NamespaceElement.Descendants(elementName).FirstOrDefault();
      if (periodElement != null)
      {
        if (IsUnitAttributeValid(periodElement, expectedUnit))
        {
          XAttribute valueAtt = periodElement.Attribute("value");
          if (valueAtt != null)
          {
            int validValue;
            if (int.TryParse(valueAtt.Value, out validValue))
            {
              result = validValue;
            }
          }
        }
      }

      return result;
    }

    private bool IsUnitAttributeValid(XElement element, string expectedUnit)
    {
      bool result = false;
      XAttribute unitAtt = element.Attribute("unit");
      if (unitAtt == null)
      {
        string message = "Element " + element.Name + " does not have a unit attribute.";
        string input = element.ToString();
        LogErrorMessage(message, input, "TLDMLNamespaceElement.IsUnitAttributeValid");
      }
      else if (unitAtt.Value.Equals(expectedUnit, StringComparison.OrdinalIgnoreCase))
      {
        result = true;
      }
      else
      {
        string message = unitAtt.Value + " was not the expected unit value: " + expectedUnit;
        string input = element.ToString();
        LogErrorMessage(message, input, "TLDMLNamespaceElement.IsUnitAttributeValid");
      }

      return result;
    }

    private void LogErrorMessage(string message, string input, string sourceFunction)
    {
      AtlantisException aex = new AtlantisException(sourceFunction, "0", message, input, null, null);
      Engine.Engine.LogAtlantisException(aex);
    }
  }
}
