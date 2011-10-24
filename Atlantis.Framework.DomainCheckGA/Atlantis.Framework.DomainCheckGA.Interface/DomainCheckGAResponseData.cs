using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;

using Atlantis.Framework.Interface;


namespace Atlantis.Framework.DomainCheckGA.Interface
{
  public class DomainCheckGAResponseData : IResponseData
  {
    private bool? _isSuccess = null;
    string _responseXml = null;
    AtlantisException _exception = null;
    WebExceptionStatus _serviceExceptionStatus = WebExceptionStatus.Success;
    Dictionary<string, DomainAttributesGA> _domainsDictionary = null;

    public DomainCheckGAResponseData(WebExceptionStatus serviceException)
    {
      _responseXml = "<checkdata></checkdata>";
      _serviceExceptionStatus = serviceException;
      _isSuccess = false;
    }

    public DomainCheckGAResponseData(string responseXml)
    {
      _responseXml = responseXml;
    }

    public DomainCheckGAResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
      _isSuccess = false;
    }

    public DomainCheckGAResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                   "DomainCheckGAResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
      _isSuccess = false;
    }

    public KeyValuePair<string, DomainAttributesGA> FirstDomain
    {
      get
      {
        if (_domainsDictionary == null)
        {
          _domainsDictionary = new Dictionary<string, DomainAttributesGA>(StringComparer.OrdinalIgnoreCase);
          PopulateFromXML();
        }

        IEnumerator<KeyValuePair<string, DomainAttributesGA>> oEnum = _domainsDictionary.GetEnumerator();
        oEnum.MoveNext();

        return oEnum.Current;
      }
    }

    public Dictionary<string, DomainAttributesGA> Domains
    {
      get
      {
        if (_domainsDictionary == null)
        {
          _domainsDictionary = new Dictionary<string, DomainAttributesGA>(StringComparer.OrdinalIgnoreCase);
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
      XmlDocument xdDoc = new XmlDocument();

      xdDoc.LoadXml(_responseXml);

      XmlNodeList xnlDomains = xdDoc.SelectNodes("/checkdata/domain");

      foreach (XmlElement xlDomain in xnlDomains)
      {
        int iAvailable = 0;
        int iSyntaxResult = 0;
        string sSyntaxDescription = String.Empty;
        string sResult = String.Empty;

        string sFullName = xlDomain.GetAttribute("name");

        sResult = xlDomain.GetAttribute("result");
        Int32.TryParse(sResult, out iAvailable);
        sResult = String.Empty;

        XmlElement xlSyntax = xlDomain.SelectSingleNode("./syntax") as XmlElement;
        if (xlSyntax != null)
        {
          sResult = xlSyntax.GetAttribute("result");
          Int32.TryParse(sResult, out iSyntaxResult);
          sResult = String.Empty;

          sSyntaxDescription = xlSyntax.GetAttribute("description");
        }

        _domainsDictionary[sFullName.ToUpper()] = new DomainAttributesGA(iAvailable, iSyntaxResult, sSyntaxDescription);
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
        _serviceExceptionStatus = value;
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

