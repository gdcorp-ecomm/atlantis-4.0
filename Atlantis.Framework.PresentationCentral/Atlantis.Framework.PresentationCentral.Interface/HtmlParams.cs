using System;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml;

namespace Atlantis.Framework.PresentationCentral.Interface
{
  /// <summary>
  /// Class to capture HTML retrieval options
  /// </summary>
  [Serializable]
  public class HtmlParams : IXmlSerializable
  {
    private Dictionary<string, string> _attributes;

    // **************************************************************** //
    /// <summary>
    /// Default constructor required for serialization
    /// </summary>

    public HtmlParams()
    {
    }

    // **************************************************************** //
    /// <summary>
    /// Constructor for normal requests
    /// </summary>
    /// <param name="transformaName">name of the transform</param>
    /// <param name="application">Arbitrary application string.</param>
    /// <param name="isSecure">Identifies secure or non-secure connection</param>
    public HtmlParams(string transformName, string application, bool isSecure)
    {
      _attributes = new Dictionary<string, string>(5);
      TransformName = transformName;
      Http = isSecure ? "https://" : "http://";
      Application = application;
    }

    // **************************************************************** //

    public string TransformName
    {
      get { return _attributes["TransformName"]; }
      set { _attributes["TransformName"] = value; }
    }

    // **************************************************************** //

    public string Application
    {
      get { return _attributes["inApp"]; }
      set { _attributes["inApp"] = value; }
    }

    // **************************************************************** //

    public string Http
    {
      get { return _attributes["http"]; }
      set { _attributes["http"] = value; }
    }

    public Dictionary<string, string> Attributes
    {
      get { return _attributes; }
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
