using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetShopper.Interface
{
  public class GetShopperRequestData : RequestData
  {

    struct InterestPref
    {
      public InterestPref(int communicationTypeId, int interestTypeId)
      {
        this.CommunicationTypeId = communicationTypeId;
        this.InterestTypeId = interestTypeId;
      }

      public int CommunicationTypeId;
      public int InterestTypeId;
    }

    string _IPAddress;
    HashSet<string> _fields = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
    List<int> _communictionPrefs = new List<int>();
    List<InterestPref> _interestPrefs = new List<InterestPref>();

    public GetShopperRequestData(string shopperId,
                              string sourceUrl,
                              string orderId,
                              string pathway,
                              int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestedBy = string.Empty;
      this._IPAddress = GetLocalAddress();
      RequestTimeout = TimeSpan.FromSeconds(14d);
    }

    public GetShopperRequestData(string shopperId,
                              string sourceUrl,
                              string orderId,
                              string pathway,
                              int pageCount,
                              string sRequestedBy)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestedBy = sRequestedBy;
      this._IPAddress = GetLocalAddress();
      RequestTimeout = TimeSpan.FromSeconds(14d);
    }

    public GetShopperRequestData(string shopperId,
                              string sourceUrl,
                              string orderId,
                              string pathway,
                              int pageCount,
                              string sRequestedBy,
                              IEnumerable<string> fields)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestedBy = sRequestedBy;
      this._IPAddress = GetLocalAddress();
      AddFields(fields);
      RequestTimeout = TimeSpan.FromSeconds(14d);
    }

    public GetShopperRequestData(string shopperId,
                              string sourceUrl,
                              string orderId,
                              string pathway,
                              int pageCount,
                              string sRequestedBy,
                              string sIPAddress)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestedBy = sRequestedBy;
      this.IPAddress = sIPAddress;
      RequestTimeout = TimeSpan.FromSeconds(14d);
    }

    public GetShopperRequestData(string shopperId,
                              string sourceUrl,
                              string orderId,
                              string pathway,
                              int pageCount,
                              string sRequestedBy,
                              string sIPAddress,
                              IEnumerable<string> fields)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestedBy = sRequestedBy;
      this.IPAddress = sIPAddress;
      AddFields(fields);
      RequestTimeout = TimeSpan.FromSeconds(14d);
    }

    public void AddFields(IEnumerable<string> fields)
    {
      foreach (string fieldName in fields)
      {
        _fields.Add(fieldName);
      }
    }

    public void AddField(string fieldName)
    {
      _fields.Add(fieldName);
    }

    public void AddInterestPref(int communicationTypeId, int interestTypeId)
    {
      _interestPrefs.Add(new InterestPref(communicationTypeId, interestTypeId));
    }

    public void AddCommunicationPref(int communicationTypeId)
    {
      _communictionPrefs.Add(communicationTypeId);
    }

    public string IPAddress
    {
      get { return _IPAddress; }
      set
      {
        _IPAddress = string.Empty;
        IPAddress address = null;
        if (System.Net.IPAddress.TryParse(value, out address))
          _IPAddress = address.ToString();
      }
    }

    public string RequestedBy { get; set; }

    string GetLocalAddress()
    {
      string result = string.Empty;

      IPAddress[] addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
      if (addresses.Length > 0)
        result = addresses[0].ToString();

      return result;
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("GetShopper is not a cacheable request.");
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("ShopperGet");
      xtwRequest.WriteAttributeString("ID", ShopperID);
      xtwRequest.WriteAttributeString("IPAddress", GetLocalAddress());
      xtwRequest.WriteAttributeString("RequestedBy", RequestedBy);

      xtwRequest.WriteStartElement("ReturnFields");
      foreach (string sField in _fields)
      {
        xtwRequest.WriteStartElement("Field");
        xtwRequest.WriteAttributeString("Name", sField);
        xtwRequest.WriteEndElement(); // Field
      }
      xtwRequest.WriteEndElement(); // ReturnFields

      xtwRequest.WriteStartElement("ReturnPreferences");
      foreach (InterestPref interest in _interestPrefs)
      {
        xtwRequest.WriteStartElement("Interest");
        xtwRequest.WriteAttributeString("CommTypeID", interest.CommunicationTypeId.ToString());
        xtwRequest.WriteAttributeString("InterestTypeID", interest.InterestTypeId.ToString());
        xtwRequest.WriteEndElement(); // Interest
      }
      foreach (int lCommTypeID in _communictionPrefs)
      {
        xtwRequest.WriteStartElement("Communication");
        xtwRequest.WriteAttributeString("CommTypeID", lCommTypeID.ToString());
        xtwRequest.WriteEndElement(); // Communication
      }
      xtwRequest.WriteEndElement(); // ReturnPreferences

      xtwRequest.WriteEndElement(); // ShopperGet

      return sbRequest.ToString();
    }

    #endregion
  }
}
