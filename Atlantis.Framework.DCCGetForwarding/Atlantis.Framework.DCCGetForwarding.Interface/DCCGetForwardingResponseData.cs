using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetForwarding.Interface
{
  public class DCCGetForwardingResponseData : IResponseData
  {
    private readonly AtlantisException _exception;

    public IDictionary<string, object> PropertiesDictionary { get; private set; }

    public bool HasForwardingRecord { get; private set; }

    public bool IsSuccess { get; private set; }

    private int? _redirectID;
    public int RedirectID
    {
      get
      {
        if(_redirectID == null)
        {
          _redirectID = 0;
          if (IsPropertyInDictionary("redirect_id"))
          {
            _redirectID = Convert.ToInt32(PropertiesDictionary["redirect_id"]);
          }
        }
        return _redirectID.Value;
      }
    }

    private int? _maskRecurringID;
    public int MaskRecurringID
    {
      get
      {
        if (_maskRecurringID == null)
        {
          _maskRecurringID = 0;
          if (IsPropertyInDictionary("mask_recurring_id"))
          {
            _maskRecurringID = Convert.ToInt32(PropertiesDictionary["mask_recurring_id"]);
          }
        }
        return _maskRecurringID.Value;
      }
    }

    private int? _redirectType;
    public int RedirectType
    {
      get
      {
        if (_redirectType == null)
        {
          _redirectType = 0;
          if (IsPropertyInDictionary("redirect_type"))
          {
            _redirectType = Convert.ToInt32(PropertiesDictionary["redirect_type"]);
          }
        }
        return _redirectType.Value;
      }
    }

    private string _domainName;
    public string DomainName
    {
      get
      {
        if (_domainName == null)
        {
          _domainName = string.Empty;
          if (IsPropertyInDictionary("domain"))
          {
            _domainName = Convert.ToString(PropertiesDictionary["domain"]);
          }
        }
        return _domainName;
      }
    }

    private string _redirectURL;
    public string RedirectURL
    {
      get
      {
        if (_redirectURL == null)
        {
          _redirectURL = string.Empty;
          if (IsPropertyInDictionary("redirect_url"))
          {
            _redirectURL = Convert.ToString(PropertiesDictionary["redirect_url"]);
          }
        }
        return _redirectURL;
      }
    }

    private string _status;
    public string Status
    {
      get
      {
        if (_status == null)
        {
          _status = string.Empty;
          if (IsPropertyInDictionary("status"))
          {
            _status = Convert.ToString(PropertiesDictionary["status"]);
          }
        }
        return _status;
      }
    }

    private string _metaTagTitle;
    public string MetaTagTitle
    {
      get
      {
        if (_metaTagTitle == null)
        {
          _metaTagTitle = string.Empty;
          if (IsPropertyInDictionary("metatag_title"))
          {
            _metaTagTitle = Convert.ToString(PropertiesDictionary["metatag_title"]);
          }
        }
        return _metaTagTitle;
      }
    }

    private string _metaTagDescription;
    public string MetaTagDescription
    {
      get
      {
        if (_metaTagDescription == null)
        {
          _metaTagDescription = string.Empty;
          if (IsPropertyInDictionary("metatag_description"))
          {
            _metaTagDescription = Convert.ToString(PropertiesDictionary["metatag_description"]);
          }
        }
        return _metaTagDescription;
      }
    }

    private string _metaTagKeyword;
    public string MetaTagKeyword
    {
      get
      {
        if (_metaTagKeyword == null)
        {
          _metaTagKeyword = string.Empty;
          if (IsPropertyInDictionary("metatag_keyword"))
          {
            _metaTagKeyword = Convert.ToString(PropertiesDictionary["metatag_keyword"]);
          }
        }
        return _metaTagKeyword;
      }
    }

    internal DCCGetForwardingResponseData(IDictionary<string, object> propertiesDictionary)
    {
      PropertiesDictionary = propertiesDictionary;
      IsSuccess = true;
      HasForwardingRecord = true;
    }

    internal DCCGetForwardingResponseData()
    {
      PropertiesDictionary = new Dictionary<string, object>(1);
      IsSuccess = true;
      HasForwardingRecord = false;
    }

    internal DCCGetForwardingResponseData(RequestData oRequestData, Exception ex)
    {
      PropertiesDictionary = new Dictionary<string, object>(1);
      IsSuccess = false;
      _exception = new AtlantisException(oRequestData,
                                         "DCCGetForwardingResponseData",
                                         ex.Message,
                                         oRequestData.ToXML());
    }

    public string ToXML()
    {
      XmlDocument requestDoc = new XmlDocument();
      requestDoc.LoadXml("<forwarding/>");

      XmlElement oRoot = requestDoc.DocumentElement;
      AddAttribute(oRoot, "domainname", DomainName);
      AddAttribute(oRoot, "redirectid", RedirectID.ToString());
      AddAttribute(oRoot, "status", Status);
      AddAttribute(oRoot, "maskrecurringid", MaskRecurringID.ToString());
      AddAttribute(oRoot, "metatagtitle", MetaTagTitle);
      AddAttribute(oRoot, "metatagdescription", MetaTagDescription);
      AddAttribute(oRoot, "metatagkeyword", MetaTagKeyword);
      AddAttribute(oRoot, "metatagredirectype", RedirectType.ToString());

      return requestDoc.InnerXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    private static void AddAttribute(XmlNode node, string sAttributeName, string sAttributeValue)
    {
      XmlAttribute attribute = node.OwnerDocument.CreateAttribute(sAttributeName);
      node.Attributes.Append(attribute);
      attribute.Value = sAttributeValue;
    }

    private bool IsPropertyInDictionary(string key)
    {
      return PropertiesDictionary.ContainsKey(key) && PropertiesDictionary[key] != null && !(PropertiesDictionary[key] is DBNull);
    }
  }
}
