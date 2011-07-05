using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Atlantis.Framework.PresentationCentral.Interface
{
  /// <summary>
  /// Class to capture HTML XsltMap options
  /// </summary>
  [Serializable]
  public class HtmlXslt : IXmlSerializable
  {
      private Dictionary<string, string> _attributes;

      public HtmlXslt()
      {
        _attributes = new Dictionary<string, string>(1);
      }

      [XmlAttribute("OverrideDateTime")]
      public string OverrideDateTime
      {
        get { return _attributes["OverrideDateTime"]; }
        set { _attributes["OverrideDateTime"] = value; }
      }

      #region IXmlSerializable Members

      public System.Xml.Schema.XmlSchema GetSchema()
      {
        return null;
      }

      public void ReadXml(XmlReader reader)
      {
        throw new NotImplementedException();
      }

      public void WriteXml(XmlWriter writer)
      {
        foreach (string key in _attributes.Keys)
        {
          string value;
          if (_attributes.TryGetValue(key, out value))
          {
            writer.WriteAttributeString(key, value);
          }
        }
      }

      #endregion
  }
}
