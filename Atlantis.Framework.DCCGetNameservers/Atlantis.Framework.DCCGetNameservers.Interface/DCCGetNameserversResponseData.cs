using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetNameservers.Interface
{
  public class DCCGetNameserversResponseData : IResponseData
  {
    private string _responseXml;
    private AtlantisException _exception;

    private Dictionary<String, List<string>> _domains = new Dictionary<string, List<string>>();

    public DCCGetNameserversResponseData(string responseXml)
    {
      _responseXml = responseXml;
      PopulateFromXML(responseXml);
    }

    public DCCGetNameserversResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public DCCGetNameserversResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                   "DCCGetDomainContactsResponseData", 
                                   ex.Message, 
                                   oRequestData.ToXML());
    }

    /*
<results>
     <method>GetNameserverInfoByDomainNameAndShopperId</method>
     <success>1</success>
<domains>
	<domain domainname="0kajsdhfjkahsdf.com" shopperid="839627" processing="success">
		<host hostname="park3.secureserver.net"/>
		<host hostname="park4.secureserver.net"/>
	</domain>
</domains></results>
    */

    public Dictionary<String, List<string>> Domains
    {
      get { return _domains; }
    }

    void PopulateFromXML(string resultXml)
    {
      XmlDocument xdDoc = new XmlDocument();
      xdDoc.LoadXml(resultXml);

      XmlElement xnSuccess = (XmlElement)xdDoc.SelectSingleNode("/results/success");
      if (xnSuccess != null)
      {
        if( xnSuccess.InnerText == "1" )
        {
          _isSuccess = true;
        }
      }

      XmlNodeList xnlDomain = xdDoc.SelectNodes("/results/domains/domain");
      foreach (XmlElement xnDomain in xnlDomain)
      {
        List<string> oNameservers = new List<string>();

        XmlNodeList xnlHost = xnDomain.SelectNodes("host");

        if(xnlHost == null || xnlHost.Count == 0)
        {
          _isSuccess = false;
        }
        else
        {
          foreach (XmlElement oHost in xnlHost)
          {
            oNameservers.Add(oHost.Attributes["hostname"].Value);
          }
        }

        _domains.Add(xnDomain.Attributes["domainname"].Value, oNameservers);
      }      
    }

    private bool _isSuccess;
    public bool IsSuccess
    {
      get { return (_exception == null && _isSuccess); }
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseXml;
    }
  }
}
