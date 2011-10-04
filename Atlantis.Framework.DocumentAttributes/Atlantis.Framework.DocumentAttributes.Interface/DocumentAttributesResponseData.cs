using System;
using System.Collections.Generic;
using System.Xml;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DocumentAttributes.Interface
{
  public class DocumentAttributesResponseData : IResponseData
  {
    private string _xml = string.Empty;
    AtlantisException _exception;

    private Dictionary<string, DocumentAttributes> _documentAttributes = new Dictionary<string, DocumentAttributes>();

    public DocumentAttributesResponseData(string xml)
    {
      _xml = xml;
      if (!string.IsNullOrEmpty(xml))
      {
        ParseDocument();
      }
    }

    public DocumentAttributesResponseData(string xml, AtlantisException exAtlantis)
      : this(xml)
    {
      _xml = xml;
      _exception = exAtlantis;
    }

    public DocumentAttributesResponseData(string xml, RequestData requestData, Exception ex)
      : this(xml)
    {
      _xml = xml;
      _exception = new AtlantisException(requestData, "DocumentAttributesResponseData", ex.Message, requestData.ToXML());
    }

    public Dictionary<string, DocumentAttributes> GetDocumentAttributes
    {
      get
      {
        return _documentAttributes;
      }
    }

    private void ParseDocument()
    {
      using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(_xml)))
      {
        try
        {
          while (reader.Read())
          {
            if (reader.NodeType == XmlNodeType.Element)
            {
              if (reader.Name.Equals("document",StringComparison.InvariantCultureIgnoreCase))
              {
                DocumentAttributes docAttr = new DocumentAttributes();
                docAttr.Id = reader.GetAttribute("id");
                docAttr.Name = reader.GetAttribute("name");
                docAttr.Title = reader.GetAttribute("title");
                DateTime.TryParse(reader.GetAttribute("last_modified"), out docAttr.LastModified);
                _documentAttributes[docAttr.Name] = docAttr;
              }
            }
          }
        }
        finally
        {
          reader.Close();
        }
      }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _xml;
    }

    #endregion
  }
}
