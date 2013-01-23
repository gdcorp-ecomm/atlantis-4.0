using System;
using System.IO;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthRetrieve.Interface
{
  public class AuthRetrieveResponseData : IResponseData
  {
    #region Properties
    private readonly AtlantisException _exception;
    private readonly string _resultXml = string.Empty;
    private readonly bool _success;
    private readonly string _shopperId = string.Empty;
    private readonly string _spid = string.Empty;
    private readonly string _ipid = string.Empty;
    private readonly XDocument _artifactDoc = new XDocument();

    [Obsolete("Use ToXML")]
    public string ResultXML
    {
      get { return _resultXml; }
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public string ShopperId
    {
      get { return _shopperId; }
    }

    public XDocument ArtifactDoc
    {
      get { return _artifactDoc; }
    }

    public string IpID
    {
      get { return _ipid; }
    }

    public string SpID
    {
      get { return _spid; }
    }
    #endregion

    public AuthRetrieveResponseData(string xml)
    {
      _success = true;
      _resultXml = xml;
      _artifactDoc = XDocument.Load(new StringReader(xml));
      foreach (var currentElement in _artifactDoc.Descendants("Request"))
      {
        foreach (var currentNode in currentElement.Nodes())
        {
          var node = currentNode as XElement;
          if (node != null)
          {
            var currAttrib = node;
            if (currAttrib.Name == "ShopperID")
            {
              _shopperId = currAttrib.Value;
              break;
            }
          }
        }
      }
      var rootNode = _artifactDoc.Root;
      if (rootNode != null)
      {
        if (rootNode.Attribute("idpid") != null)
        {
          _ipid = rootNode.Attribute("idpid").Value;
        }
        if (rootNode.Attribute("spid") != null)
        {
          _spid = rootNode.Attribute("spid").Value;
        }
      }

    }

    public AuthRetrieveResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public AuthRetrieveResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData, "AuthRetrieveResponseData", exception.Message, requestData.ToXML());
    }

    #region IResponseData Members
    public string ToXML()
    {
      return _resultXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion
  }
}
