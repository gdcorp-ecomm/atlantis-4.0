using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SearchShoppers.Interface
{
  public class SearchShoppersRequestData : RequestData
  {

    string _requestedBy;
    string _ipAddress;
    readonly List<SearchField> _searchFields = new List<SearchField>();
    readonly List<string> _returnFields = new List<string>();

    public SearchShoppersRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount, 
                                  string requestedBy)
                                  : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _requestedBy = requestedBy;
      _ipAddress = "";
    }

    public SearchShoppersRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string requestedBy,
                                  IEnumerable<SearchField> searchFields,
                                  IEnumerable<string> returnFields)
                                  : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _requestedBy = requestedBy;
      _ipAddress = "";
      AddSearchFields(searchFields);
      AddReturnFields(returnFields);
    }

    public SearchShoppersRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string requestedBy, 
                                  string ipAddress)
                                  : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _requestedBy = requestedBy;
      IPAddress = ipAddress;
    }

    public SearchShoppersRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string requestedBy,
                                  string ipAddress,
                                  IEnumerable<SearchField> searchFields,
                                  IEnumerable<string> returnFields)
                                  : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _requestedBy = requestedBy;
      IPAddress = ipAddress;
      AddSearchFields(searchFields);
      AddReturnFields(returnFields);
    }

    public void AddSearchField(string name, string matchValue)
    {
      _searchFields.Add(new SearchField(name, matchValue));
    }

    public void AddSearchFields(IEnumerable<SearchField> searchFields)
    {
      _searchFields.AddRange(searchFields);
    }

    public void AddReturnField(string name)
    {
      _returnFields.Add(name);
    }

    public void AddReturnFields(IEnumerable<string> returnFields)
    {
      _returnFields.AddRange(returnFields);
    }

    public string RequestedBy
    {
      get { return _requestedBy; }
      set { _requestedBy = value; }
    }

    public string IPAddress
    {
      get { return GetLocalAddress(); }
      set
      {
        _ipAddress = "";
        IPAddress address;
        if (System.Net.IPAddress.TryParse(value, out address))
          _ipAddress = address.ToString();
      }
    }

    string GetLocalAddress()
    {
      if(_ipAddress.Length == 0)
      {
        IPAddress[] addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

        if (addresses.Length > 0)
          _ipAddress = addresses[0].ToString();
      }

      return _ipAddress;
    }

    #region RequestData  Members

    public override string GetCacheMD5()
    {
      throw new Exception("SearchShoppers is not a cacheable request.");
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("ShopperSearch");
      xtwRequest.WriteAttributeString("IPAddress", GetLocalAddress());
      xtwRequest.WriteAttributeString("RequestedBy", _requestedBy);

      xtwRequest.WriteStartElement("SearchFields");
      foreach (SearchField field in _searchFields)
      {
        xtwRequest.WriteStartElement("Field");
        xtwRequest.WriteAttributeString("Name", field.FieldName);
        xtwRequest.WriteValue(field.MatchValue);
        xtwRequest.WriteEndElement(); // Field
      }
      xtwRequest.WriteEndElement();  // SearchFields

      xtwRequest.WriteStartElement("ReturnFields");
      foreach (string sField in _returnFields)
      {
        xtwRequest.WriteStartElement("Field");
        xtwRequest.WriteAttributeString("Name", sField);
        xtwRequest.WriteEndElement(); // Field
      }

      xtwRequest.WriteEndElement(); // ReturnFields
      xtwRequest.WriteEndElement(); // ShopperSearch

      return sbRequest.ToString();
    }

    #endregion

  }
}
