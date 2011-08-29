using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.NameserverCheck.Interface
{
  public class NameserverCheckResponseData : IResponseData
  {
    string _responseXml = string.Empty;
    AtlantisException _exception = null;
    Dictionary<string, NameserverAttributes> _nameServers = null;
    bool? _isSuccess = null;

    public Dictionary<string, NameserverAttributes> Nameservers
    {
      get
      {
        if (_nameServers == null)
        {
          _nameServers = new Dictionary<string, NameserverAttributes>(StringComparer.InvariantCultureIgnoreCase);
          PopulateFromXML();
        }

        return _nameServers;
      }
    }

    public bool IsSuccess
    {
      get 
      {
        if (_isSuccess == null)
        {
          _isSuccess = (_responseXml.IndexOf("<host", StringComparison.OrdinalIgnoreCase) > -1);
        }
        return _isSuccess.Value;
      }
    }

    public NameserverCheckResponseData(string responseXml)
    {
      _responseXml = responseXml;
    }

    public NameserverCheckResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
      _isSuccess = false;
    }

    public NameserverCheckResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                   "NameserverCheckResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
      _isSuccess = false;
    }

    private void PopulateFromXML()
    {
      XmlDocument xdDoc = new XmlDocument();

      xdDoc.LoadXml(_responseXml);

      XmlNodeList xnlNameservers = xdDoc.SelectNodes("/checkdata/host");

      foreach (XmlElement xlNameserver in xnlNameservers)
      {
        int iAvailable = 0;
        int iSyntaxResult = 0;
        string sSyntaxDescription = String.Empty;
        string sResult = String.Empty;

        string sFullName = xlNameserver.GetAttribute("name");

        sResult = xlNameserver.GetAttribute("result");
        Int32.TryParse(sResult, out iAvailable);
        sResult = String.Empty;

        XmlElement xlSyntax = xlNameserver.SelectSingleNode("./syntax") as XmlElement;
        if (xlSyntax != null)
        {
          sResult = xlSyntax.GetAttribute("result");
          Int32.TryParse(sResult, out iSyntaxResult);
          sResult = String.Empty;

          sSyntaxDescription = xlSyntax.GetAttribute("description");
        }

        _nameServers.Add(sFullName.ToUpper(), new NameserverAttributes(iAvailable, iSyntaxResult, sSyntaxDescription));
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
