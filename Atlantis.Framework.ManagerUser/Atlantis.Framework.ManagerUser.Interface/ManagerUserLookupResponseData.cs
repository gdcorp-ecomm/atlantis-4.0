using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;

namespace Atlantis.Framework.ManagerUser.Interface
{
  public class ManagerUserLookupResponseData : IResponseData
  {
    private const string _STATUSSUCCESS = "0";
    private AtlantisException _ex;
    private string _responseXml;
    private string _status = string.Empty;
    private string _error = string.Empty;
    private string _managerUserID = string.Empty;
    private string _managerLoginName = string.Empty;

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

    public ManagerUserLookupResponseData(string responseXml)
    {
      _responseXml = responseXml;
      ParseXml();
    }

    private void ParseXml()
    {
      XmlDocument responseDoc = new XmlDocument();
      try
      {
        responseDoc.LoadXml(_responseXml);
        XmlElement userNode = responseDoc.SelectSingleNode("//User") as XmlElement;
        if (userNode != null)
        {
          _status = userNode.GetAttribute("status");
          _error = userNode.GetAttribute("error");

          if (_status == _STATUSSUCCESS)
          {
            XmlElement mappingNode = userNode.SelectSingleNode("./Mapping") as XmlElement;
            if (mappingNode != null)
            {
              _managerLoginName = mappingNode.GetAttribute("loginName");
              _managerUserID = mappingNode.GetAttribute("userID");
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
          string.Empty, string.Empty, ex.Message, _responseXml,
          string.Empty, string.Empty, string.Empty, string.Empty, 0);
      }
    }

    public ManagerUserLookupResponseData(string responseXml, AtlantisException ex)
    {
      _responseXml = responseXml;
      _ex = ex;
    }

    public ManagerUserLookupResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      _ex = new AtlantisException(requestData,
                                   "ManagerUserLookupResponseData",
                                   ex.Message,
                                   requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }
}
