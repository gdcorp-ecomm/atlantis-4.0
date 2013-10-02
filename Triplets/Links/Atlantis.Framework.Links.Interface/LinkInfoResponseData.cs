using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Atlantis.Framework.Collections;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Links.Interface
{
  public class LinkInfoResponseData : IResponseData
  {
    public LinkInfoResponseData(IDictionary<string, ILinkInfo> dictLinkInfo, IDictionary<string, string> dictLinkTypesByBaseUrl)
    {
      Links = new ReadOnlyDictionary<string, ILinkInfo>(dictLinkInfo);
      LinkTypesByBaseUrl = new ReadOnlyDictionary<string, string>(dictLinkTypesByBaseUrl);
      _Xml = new Lazy<string>(() =>
        {
          var result = new StringBuilder(1024);
          var xtwResult = new XmlTextWriter(new StringWriter(result));

          xtwResult.WriteStartElement("Links");

          if (Links != null)
          {
            //Create our own namespaces for the output
            var ns = new XmlSerializerNamespaces();
            ns.Add(String.Empty, String.Empty);

            foreach (KeyValuePair<string, ILinkInfo> oPair in Links)
            {
              xtwResult.WriteStartElement("Link");
              xtwResult.WriteAttributeString("type", oPair.Key);
              var slz = new XmlSerializer(oPair.Value.GetType());
              slz.Serialize(xtwResult, oPair.Value, ns);
              xtwResult.WriteEndElement();
            }
          }

          xtwResult.WriteEndElement();

          return result.ToString();
        });
    }

    public IDictionary<string, ILinkInfo> Links { get; private set; }
    public IDictionary<string, string> LinkTypesByBaseUrl { get; private set; }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return null;
    }

    private Lazy<string> _Xml;
    public string ToXML()
    {
      return _Xml.Value;
    }

    #endregion

  }
}
