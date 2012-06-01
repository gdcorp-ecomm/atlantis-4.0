using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using System.Linq;
using System.Xml.Linq;
using System.IO;

namespace Atlantis.Framework.AuthRetrieve.Interface
{
  public class AuthRetrieveResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;
    private string _shopperId = string.Empty;
    private string _spid = string.Empty;
    private string _ipid = string.Empty;

    XDocument _artifactDoc = new XDocument();


    public string ResultXML
    {
      get
      {
        return _resultXML;
      }
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public string ShopperId
    {
      get
      {
        return _shopperId;
      }
    }

    public XDocument ArtifactDoc
    {
      get
      {
        return _artifactDoc;
      }
    }

    public string IpID
    {
      get
      {
        return _ipid;
      }
    }

    public string SpID
    {
      get
      {
        return _spid;
      }
    }

    public AuthRetrieveResponseData(string xml)
    {
      this._success = true;
      this._resultXML=xml;
      _artifactDoc = XDocument.Load(new StringReader(xml));
      foreach (XElement currentElement in _artifactDoc.Descendants("Request"))
      {
        foreach (XNode currentNode in currentElement.Nodes())
        {
          if (currentNode is XElement)
          {
            XElement currAttrib = (XElement)currentNode;
            if (currAttrib.Name == "ShopperID")
            {
              _shopperId = currAttrib.Value;
              break;
            }
          }
        }

      }
      XElement rootNode = _artifactDoc.Root;
      if (rootNode.Attribute("idpid") != null)
      {
        _ipid = _artifactDoc.Root.Attribute("idpid").Value;
      }
      if (rootNode.Attribute("spid") != null)
      {
        _spid = _artifactDoc.Root.Attribute("spid").Value;
      }

    }

     public AuthRetrieveResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;      
    }

    public AuthRetrieveResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "AuthRetrieveResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
