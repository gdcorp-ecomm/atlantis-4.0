using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Atlantis.Framework.MSA.Interface
{
  [DataContract]
  public class Message
  {
    [DataMember(Name = "info")]
    public MessageInfo Info { get; set; }

    [DataMember(Name = "body")]
    public string Body { get; set; }

    [XmlIgnore]
    public string _cdataBodyStr;

    [XmlElement(ElementName = "body_cdata")]
    public XmlNode BodyCData
    {
      get
      {
        string innerXml = string.Concat("<![CDATA[", _cdataBodyStr.Replace("]]>", "]]]]><![CDATA[>"), "]]>");

        XmlDocument doc = new XmlDocument();
        XmlDocumentFragment fragment = doc.CreateDocumentFragment();
        fragment.InnerXml = innerXml;
        return fragment;
      }
      set { }
    }
  }
}
