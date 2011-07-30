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
      public InterestPref(int lComm, int lInterest)
      {
        this.lCommTypeID = lComm;
        this.lInterestTypeID = lInterest;
      }

      public int lCommTypeID;
      public int lInterestTypeID;
    }

    string _IPAddress;
    List<string> _fields = new List<string>();
    List<int> _communictionPrefs = new List<int>();
    List<InterestPref> _interestPrefs = new List<InterestPref>();

    public GetShopperRequestData(string sShopperID, 
                              string sSourceURL, 
                              string sOrderID, 
                              string sPathway, 
                              int iPageCount) 
                              : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestedBy = "";
      this._IPAddress = GetLocalAddress();
    }

    public GetShopperRequestData(string sShopperID, 
                              string sSourceURL, 
                              string sOrderID, 
                              string sPathway, 
                              int iPageCount, 
                              string sRequestedBy) 
                              : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount) 
    {
      RequestedBy = sRequestedBy;
      this._IPAddress = GetLocalAddress();
    }

    public GetShopperRequestData(string sShopperID, 
                              string sSourceURL, 
                              string sOrderID, 
                              string sPathway, 
                              int iPageCount, 
                              string sRequestedBy,
                              IEnumerable<string> fields)
                              : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount) 
    {
      RequestedBy = sRequestedBy;
      this._IPAddress = GetLocalAddress();
      AddFields(fields);
    }

    public GetShopperRequestData(string sShopperID, 
                              string sSourceURL, 
                              string sOrderID, 
                              string sPathway, 
                              int iPageCount, 
                              string sRequestedBy, 
                              string sIPAddress)
                              : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount) 
    {
      RequestedBy = sRequestedBy;
      this.IPAddress = sIPAddress;
    }

    public GetShopperRequestData(string sShopperID, 
                              string sSourceURL, 
                              string sOrderID, 
                              string sPathway, 
                              int iPageCount, 
                              string sRequestedBy, 
                              string sIPAddress,
                              IEnumerable<string> fields)
                              : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount) 
    {
      RequestedBy = sRequestedBy;
      this.IPAddress = sIPAddress;
      AddFields(fields);
    }

    public void AddFields(IEnumerable<string> fields)
    {
      _fields.AddRange(fields);
    }

    public void AddField(string sField)
    {
      _fields.Add(sField);
    }

    public void AddInterestPref(int lCommTypeID, int lInterestTypeID)
    {
      _interestPrefs.Add(new InterestPref(lCommTypeID, lInterestTypeID));
    }

    public void AddCommunicationPref(int lCommTypeID)
    {
      _communictionPrefs.Add(lCommTypeID);
    }

    public string IPAddress
    {
      get { return _IPAddress; }
      set
      {
        _IPAddress = "";
        IPAddress address = null;
        if (System.Net.IPAddress.TryParse(value, out address))
          _IPAddress = address.ToString();
      }
    }

    public string RequestedBy { get; set; }

    string GetLocalAddress()
    {
      string sLocalAddress = "";

      IPAddress[] addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

      if (addresses.Length > 0)
        sLocalAddress = addresses[0].ToString();

      return sLocalAddress;
    }

    #region RequestData Members
    
    public override string  GetCacheMD5()
    {
      throw new  Exception("GetShopper is not a cacheable request.");
    }

    public override  string ToXML()
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
        xtwRequest.WriteAttributeString("CommTypeID", interest.lCommTypeID.ToString());
        xtwRequest.WriteAttributeString("InterestTypeID", interest.lInterestTypeID.ToString());
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
