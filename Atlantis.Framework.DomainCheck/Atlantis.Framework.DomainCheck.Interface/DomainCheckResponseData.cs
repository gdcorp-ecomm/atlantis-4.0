using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using Atlantis.Framework.Interface;


namespace Atlantis.Framework.DomainCheck.Interface
{
  public class DomainCheckResponseData : IResponseData
  {
    private bool? _isSuccess = null;
    string _responseXml = null;
    AtlantisException _exception = null;
    WebExceptionStatus _serviceExceptionStatus = WebExceptionStatus.Success;
    Dictionary<string, DomainAttributes> _domainsDictionary = null;

    public DomainCheckResponseData(WebExceptionStatus serviceException)
    {
      _responseXml = "<checkdata></checkdata>";
      _serviceExceptionStatus = serviceException;
      _isSuccess = false;
    }

    public DomainCheckResponseData(string responseXml)
    {
      _responseXml = responseXml;
    }

    public DomainCheckResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
      _isSuccess = false;
    }

    public DomainCheckResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData, "DomainCheckResponseData", ex.Message, oRequestData.ToXML());
      _isSuccess = false;
    }

    public KeyValuePair<string, DomainAttributes> FirstDomain
    {
      get
      {
        if (_domainsDictionary == null)
        {
          _domainsDictionary = new Dictionary<string, DomainAttributes>(StringComparer.OrdinalIgnoreCase);
          PopulateFromXML();
        }

        IEnumerator<KeyValuePair<string, DomainAttributes>> oEnum = _domainsDictionary.GetEnumerator();
        oEnum.MoveNext();
        return oEnum.Current;
      }
    }

    public Dictionary<string, DomainAttributes> Domains
    {
      get
      {
        if (_domainsDictionary == null)
        {
          _domainsDictionary = new Dictionary<string, DomainAttributes>(StringComparer.OrdinalIgnoreCase);
          PopulateFromXML();
        }

        return _domainsDictionary;
      }
    }


    /*
     * VALIDSYNTAX RULES:
     * returns one of the following codes:
     * 1000 - No errors
     * 2101 - Minimum length error
     * 2102 - Maximum length error
     * 2111 - Invalid character(s) error
     * 2112 - Invalid TLD error
     * 2121 - Leading hyhen error
     * 2122 - Trailing hyphen error
     * 2123 - Hypen 34 error (hyphen in the 3rd and 4th position)
     * 2131 - Profanity error
     * 2141 - All numeric error
     * 2151 - Reserved name error
     * 
     * AVAILABLE RULES:
     * returns one of the following codes:
     * AVAILABLE    = 1000
     * UNAVAILABLE  = 1001
     * INVALID      = 1002
     * RES_RRP      = 1003 -- don't worry about this one
     * RES_EPP      = 1004 -- don't worry about this one
     * 
     * */

    private void PopulateFromXML()
    {
      var xdDoc = new XmlDocument();
      xdDoc.LoadXml(_responseXml);
      var xnlDomains = xdDoc.SelectNodes("/checkdata/domain");

      foreach (XmlElement xlDomain in xnlDomains)
      {
        var availableCode = 0;
        var syntaxCode = 0;
        var syntaxDescription = String.Empty;
        var attribute = String.Empty;
        var wasTyped = false;
        var idnScript = "eng";
        var languageId = "35";
        int internalTierId;

        var name = xlDomain.GetAttribute("find");

        var punyCode = xlDomain.GetAttribute("name");

        attribute = xlDomain.GetAttribute("result");
        Int32.TryParse(attribute, out availableCode);
        attribute = String.Empty;

        attribute = xlDomain.GetAttribute("internaltierid");
        
        if(!int.TryParse(attribute, out internalTierId))
        {
          internalTierId = -1;
        }

        attribute = String.Empty;

        var strTyped = xlDomain.GetAttribute("wasTyped");

        if (!string.IsNullOrEmpty(strTyped))
        {
          wasTyped = strTyped == "1";
        }

        attribute = xlDomain.GetAttribute("idnScript");
        
        if (!string.IsNullOrEmpty(attribute))
        {
          idnScript = attribute;
        }

        attribute = xlDomain.GetAttribute("languageid");

        if (!string.IsNullOrEmpty(attribute))
        {
          languageId = attribute;
        }

        var xlSyntax = xlDomain.SelectSingleNode("./syntax") as XmlElement;

        if (xlSyntax != null)
        {
          attribute = xlSyntax.GetAttribute("result");
          Int32.TryParse(attribute, out syntaxCode);
          attribute = String.Empty;
          syntaxDescription = xlSyntax.GetAttribute("description");
        }

        _domainsDictionary[name] = new DomainAttributes(availableCode, syntaxCode, syntaxDescription, wasTyped, punyCode, idnScript, languageId, internalTierId);
      }
    }

    public bool IsSuccess
    {
      get 
      {
        if (_isSuccess == null)
        {
          _isSuccess = (_responseXml.IndexOf("<domain", StringComparison.OrdinalIgnoreCase) > -1);
        }

        return _isSuccess.Value; 
      }
    }

    public WebExceptionStatus ServiceExceptionStatus
    {
      get 
      {
        return _serviceExceptionStatus; 
      }
      set
      {
        _serviceExceptionStatus=value;
      }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseXml;
    }

    #endregion

  }
}

