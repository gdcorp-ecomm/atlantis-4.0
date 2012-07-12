using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommLOCAccounts.Interface
{
  public class EcommLOCAccountsResponseData : IResponseData
  {
    private AtlantisException _exception;
    private Dictionary<int, string> _locAccounts;

    private bool _success;
    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    private string _responseXML;
    public string ResponseXML
    {
      get
      {
        return _responseXML;
      }
    }

    public EcommLOCAccountsResponseData(bool success, string responseXML)
    {
      _responseXML = responseXML;
      _success = success;
    }

    public EcommLOCAccountsResponseData(AtlantisException aex)
    {
      _success = false;
      _exception = aex;
    }

    public EcommLOCAccountsResponseData(RequestData request, Exception ex)
    {
      _success = false;
      _exception = new AtlantisException(request, "EcommLOCAccountsResponseData", ex.Message, string.Empty);
    }

    public Dictionary<int, string> LOCAccounts
    {
      get
      {
        _locAccounts = new Dictionary<int, string>(2);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(_responseXML);

        XmlNodeList nodelist = xmlDoc.SelectNodes("Accounts/Account");
        int accID = 0;
        if (nodelist != null && nodelist.Count > 0)
        {
          foreach (XmlNode node in nodelist)
          {
            if (int.TryParse(node.Attributes["accountID"].Value, out accID) && !_locAccounts.ContainsKey(accID) && accID > 0)
            {
              _locAccounts.Add(accID, node.Attributes["maskedAccountNumber"].Value);
            }
          }
        }

        return _locAccounts;
      }
    }



    #region IResponseData Members
    public string ToXML()
    {
      XmlSerializer serializer = new XmlSerializer(typeof(EcommLOCAccountsResponseData));
      StringWriter writer = new StringWriter();

      serializer.Serialize(writer, this);

      return writer.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion
  }
}
