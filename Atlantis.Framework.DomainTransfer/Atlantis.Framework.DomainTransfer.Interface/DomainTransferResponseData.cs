using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainTransfer.Interface
{
  public class DomainTransferResponseData : IResponseData
  {
    private bool? _isSuccess = null;
    string _responseXml = null;
    AtlantisException _exception = null;
    WebExceptionStatus _serviceExceptionStatus = WebExceptionStatus.Success;
    Dictionary<string, DomainTransferAttributes> _domainsDictionary = null;

    public DomainTransferResponseData(WebExceptionStatus serviceException)
    {
      _responseXml = "<checkdata></checkdata>";
      _serviceExceptionStatus = serviceException;
      _isSuccess = false;
    }

    public DomainTransferResponseData(string responseXml)
    {
      _responseXml = responseXml;
    }

    public DomainTransferResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public DomainTransferResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                   "DomainTransferResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
      _isSuccess = false;
    }

    public KeyValuePair<string, DomainTransferAttributes> FirstDomain
    {
      get
      {
        if (_domainsDictionary == null)
        {
          _domainsDictionary = new Dictionary<string, DomainTransferAttributes>(StringComparer.OrdinalIgnoreCase);
          PopulateFromXML();
        }

        IEnumerator<KeyValuePair<string, DomainTransferAttributes>> oEnum = _domainsDictionary.GetEnumerator();
        oEnum.MoveNext();

        return oEnum.Current;
      }
    }

    public Dictionary<string, DomainTransferAttributes> Domains
    {
      get
      {
        if (_domainsDictionary == null)
        {
          _domainsDictionary = new Dictionary<string, DomainTransferAttributes>(StringComparer.OrdinalIgnoreCase);
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

    /******************************************************************************/

    private void PopulateFromXML()
    {
      XmlDocument xdDoc = new XmlDocument();
      
      xdDoc.LoadXml(_responseXml);

      XmlNodeList xnlDomains = xdDoc.SelectNodes("/checkdata/domain");

      foreach (XmlElement xlDomain in xnlDomains)
      {
        int availableCode = 0; 
        int syntaxCode = 0;
        string syntaxDescription = String.Empty;
        int databaseCode = 0;
        int whoIsCode = 0;
        string whoIsDescription = string.Empty;
        string whoIsStatus = string.Empty;
        string whoIsRegistrar = string.Empty;
        DateTime whoIsExpiration = DateTime.MinValue;

        string domainName = xlDomain.GetAttribute("name").ToUpperInvariant();

        string resultCode = xlDomain.GetAttribute("result");
        Int32.TryParse(resultCode, out availableCode);

        XmlElement xlSyntax = xlDomain.SelectSingleNode("./syntax") as XmlElement;
        if (xlSyntax != null)
        {
          string syntaxResult = xlSyntax.GetAttribute("result");
          Int32.TryParse(syntaxResult, out syntaxCode);
          syntaxDescription = xlSyntax.GetAttribute("description");
        }

        XmlElement xlDatabase = xlDomain.SelectSingleNode("./db") as XmlElement;
        if (xlDatabase != null)
        {
          string databaseResult = xlDatabase.GetAttribute("result");
          Int32.TryParse(databaseResult, out databaseCode);
        }

        XmlElement xlWhoIs = xlDomain.SelectSingleNode("./whois") as XmlElement;
        if (xlWhoIs != null)
        {
          string whoIsResult = xlWhoIs.GetAttribute("result");
          Int32.TryParse(whoIsResult, out whoIsCode);
          whoIsDescription = xlWhoIs.GetAttribute("description");
          whoIsRegistrar = xlWhoIs.GetAttribute("registrar");
          whoIsStatus = xlWhoIs.GetAttribute("status");
          string whoIsExpirationText = xlWhoIs.GetAttribute("exp");
          whoIsExpiration = ParseWhoIsExpiration(whoIsExpirationText, domainName);
        }

        DomainTransferAttributes domainAttributes = new DomainTransferAttributes(
          availableCode, syntaxCode, syntaxDescription, databaseCode,
          whoIsCode, whoIsDescription, whoIsStatus, whoIsRegistrar, whoIsExpiration);

        _domainsDictionary.Add(domainName, domainAttributes);
      }
    }

    private DateTime ParseWhoIsExpiration(string whoIsExpirationText, string domainName)
    {
      DateTime result = DateTime.MinValue;

      if (whoIsExpirationText.Length > 0)
      {
        Regex utcDateEx = new Regex("\\d{4}-.*?Z", RegexOptions.Compiled);
        Match utcMatch = utcDateEx.Match(whoIsExpirationText);
        if (utcMatch.Success)
        {
          DateTime.TryParse(whoIsExpirationText,out result);
        }
        else if (domainName.EndsWith(".CA"))
        {
          DateTime.TryParse(whoIsExpirationText, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeLocal, out result);
        }
        else
        {
          string parseText = whoIsExpirationText.Trim();
          if ((domainName.EndsWith(".US") || (domainName.EndsWith(".BIZ"))))
          {
            parseText = whoIsExpirationText.Substring(8, 2) + "-" +
              whoIsExpirationText.Substring(4, 3) + "-" +
              whoIsExpirationText.Substring(whoIsExpirationText.Length - 4, 4);
          }

          string[] parts = parseText.Split('-');
          if (parts.Length == 3)
          {
            switch (parts[1].ToLowerInvariant())
            {
              case "jan": parts[1] = "1"; break;
              case "feb": parts[1] = "2"; break;
              case "mar": parts[1] = "3"; break;
              case "apr": parts[1] = "4"; break;
              case "may": parts[1] = "5"; break;
              case "jun": parts[1] = "6"; break;
              case "jul": parts[1] = "7"; break;
              case "aug": parts[1] = "8"; break;
              case "sep": parts[1] = "9"; break;
              case "oct": parts[1] = "10"; break;
              case "nov": parts[1] = "11"; break;
              case "dec": parts[1] = "12"; break;
            }
            string trimedString = parts[2].Trim();
            string year = trimedString;
            if (trimedString.Length > 4)
            {
              year = trimedString.Substring(0, 4);
            }
            parseText = parts[1] + "/" + parts[0] + "/" + year;

            DateTime.TryParse(parseText, out result);
          }
        }
      }

      return result;
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
