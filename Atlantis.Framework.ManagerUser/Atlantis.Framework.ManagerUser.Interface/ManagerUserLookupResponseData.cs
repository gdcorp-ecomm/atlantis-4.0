using System;
using Atlantis.Framework.Interface;
using System.Xml.Linq;
using System.Linq;

namespace Atlantis.Framework.ManagerUser.Interface
{
  public class ManagerUserLookupResponseData : IResponseData
  {
    private const string _STATUSSUCCESS = "0";
    private AtlantisException _ex;
    private XDocument _responseXml;
    private string _status = string.Empty;
    private string _error = string.Empty;
    private string _managerUserID = string.Empty;
    private string _managerLoginName = string.Empty;
    private string _managerFullName = string.Empty;

    public string Status
    {
      get { return _status; }
    }

    public string Error
    {
      get { return _error; }
    }

    public bool IsSuccess
    {
      get { return _status == _STATUSSUCCESS; }
    }

    public string ManagerUserId
    {
      get { return _managerUserID; }
    }

    public string ManagerLoginName
    {
      get { return _managerLoginName; }
    }

    public string ManagerFullName
    {
      get { return _managerFullName; }
    }

    public XDocument ResultXml
    {
      get { return _responseXml; }
    }

    public ManagerUserLookupResponseData(string responseXml)
    {
      try
      {
        _responseXml = XDocument.Parse(responseXml);
        ParseXml();
      }
      catch (Exception ex)
      {
        _ex = new AtlantisException(
          "ManagerUserLookup.ctor",
          string.Empty, string.Empty, ex.Message, responseXml,
          string.Empty, string.Empty, string.Empty, string.Empty, 0);
      }
    }

    private void ParseXml()
    {
      try
      {
        XElement userElement = _responseXml.Descendants("User").FirstOrDefault();
        if (userElement != null)
        {
          _status = userElement.GetAttributeValue("status");
          _error = userElement.GetAttributeValue("error");

          if (_status == _STATUSSUCCESS)
          {
            XElement mappingNode = userElement.Descendants("Mapping").FirstOrDefault();
            if (mappingNode != null)
            {
              _managerLoginName = mappingNode.GetAttributeValue("loginName");
              _managerUserID = mappingNode.GetAttributeValue("userID");
              _managerFullName = mappingNode.GetAttributeValue("fullName");
            }
            else
            {
              _status = "NO MAPPING NODE";
            }
          }
        }
      }
      catch (Exception ex)
      {
        _ex = new AtlantisException(
          "ManagerUserLookup.ParseXml",
          string.Empty, string.Empty, ex.Message, _responseXml.ToString(),
          string.Empty, string.Empty, string.Empty, string.Empty, 0);
      }
    }

    public ManagerUserLookupResponseData(string responseXml, AtlantisException ex)
    {
      _responseXml = null;
      _ex = ex;
    }

    public ManagerUserLookupResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = null;
      _ex = new AtlantisException(requestData,
                                   "ManagerUserLookupResponseData",
                                   ex.Message,
                                   requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      string result = (_responseXml != null) ? _responseXml.ToString() : string.Empty;
      return result;
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }
}
