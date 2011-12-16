using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.UpdateShopper.Interface
{
  public class UpdateShopperRequestData : RequestData
  {

    public class UpdateField
    {
      private string m_sFieldName;
      private string m_sMatchValue;

      public UpdateField(string sName, string sValue)
      {
        this.m_sFieldName = sName;
        this.m_sMatchValue = sValue;
      }

      public string FieldName
      {
        get { return m_sFieldName; }
      }

      public string MatchValue
      {
        get { return m_sMatchValue; }
      }
    }


    string m_sRequestedBy;
    string m_sIPAddress;
    List<UpdateField> m_lstUpdateFields = new List<UpdateField>();
    Dictionary<string, System.Collections.Hashtable> m_UpdatePreferences = new Dictionary<string, System.Collections.Hashtable>();
    public UpdateShopperRequestData(string sShopperID,
                                  string sSourceURL,
                                  string sOrderID,
                                  string sPathway,
                                  int iPageCount, 
                                  string sRequestedBy)
                                  : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      m_sRequestedBy = sRequestedBy;
      m_sIPAddress = "";
      RequestTimeout = new TimeSpan(0, 0, 10);
    }


    public UpdateShopperRequestData(string sShopperID,
                                  string sSourceURL,
                                  string sOrderID,
                                  string sPathway,
                                  int iPageCount,
                                  string sRequestedBy,
                                  IEnumerable<UpdateField> UpdateFields)
                                  : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      m_sRequestedBy = sRequestedBy;
      m_sIPAddress = "";
      AddUpdateFields(UpdateFields);
      RequestTimeout = new TimeSpan(0, 0, 10);
    }


    public UpdateShopperRequestData(string sShopperID,
                                  string sSourceURL,
                                  string sOrderID,
                                  string sPathway,
                                  int iPageCount,
                                  string sRequestedBy, 
                                  string sIPAddress)
                                  : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      m_sRequestedBy = sRequestedBy;
      this.IPAddress = sIPAddress;
      RequestTimeout = new TimeSpan(0, 0, 10);
    }


    public UpdateShopperRequestData(string sShopperID,
                                  string sSourceURL,
                                  string sOrderID,
                                  string sPathway,
                                  int iPageCount,
                                  string sRequestedBy,
                                  string sIPAddress,
                                  IEnumerable<UpdateField> UpdateFields)
                                  : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      m_sRequestedBy = sRequestedBy;
      this.IPAddress = sIPAddress;
      AddUpdateFields(UpdateFields);
      RequestTimeout = new TimeSpan(0, 0, 10);
    }

    public void AddUpdateField(string sFieldName, string sMatchValue)
    {
      m_lstUpdateFields.Add(new UpdateField(sFieldName, sMatchValue));
    }

    public void AddUpdateFields(IEnumerable<UpdateField> UpdateFields)
    {
      m_lstUpdateFields.AddRange(UpdateFields);
    }

    public void AddPreference(string name, System.Collections.Hashtable attributes)
    {
      m_UpdatePreferences.Add(name, attributes);
    }

    public string RequestedBy
    {
      get { return m_sRequestedBy; }
      set { m_sRequestedBy = value; }
    }


    public string IPAddress
    {
      get { return GetLocalAddress(); }
      set
      {
        m_sIPAddress = "";
        IPAddress address;
        if (System.Net.IPAddress.TryParse(value, out address))
          m_sIPAddress = address.ToString();
      }
    }


    string GetLocalAddress()
    {
      if(m_sIPAddress.Length == 0)
      {
        IPAddress[] addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

        if (addresses.Length > 0)
          m_sIPAddress = addresses[0].ToString();
      }

      return m_sIPAddress;
    }


    #region RequestData  Members


    public override string GetCacheMD5()
    {
      throw new Exception("UpdateShoppers is not a cacheable request.");
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("ShopperUpdate");
      xtwRequest.WriteAttributeString("IPAddress", GetLocalAddress());
      xtwRequest.WriteAttributeString("RequestedBy", m_sRequestedBy);
      xtwRequest.WriteAttributeString("ID", ShopperID);

      if (m_lstUpdateFields.Count > 0)
      {
        xtwRequest.WriteStartElement("Fields");
        foreach (UpdateField field in m_lstUpdateFields)
        {
          xtwRequest.WriteStartElement("Field");
          xtwRequest.WriteAttributeString("Name", field.FieldName);
          xtwRequest.WriteValue(field.MatchValue);
          xtwRequest.WriteEndElement(); // Field
        }
        xtwRequest.WriteEndElement();  // UpdateFields
      }
      if (m_UpdatePreferences.Count > 0)
      {
        xtwRequest.WriteStartElement("Preferences");
        foreach (KeyValuePair<string,System.Collections.Hashtable> field in m_UpdatePreferences)
        {
          xtwRequest.WriteStartElement(field.Key);
          foreach (System.Collections.DictionaryEntry attribs in field.Value)
          {
            xtwRequest.WriteAttributeString(attribs.Key as string, attribs.Value as string);
          }
          xtwRequest.WriteEndElement();
        }
        xtwRequest.WriteEndElement();  // UpdateFields
      }

      xtwRequest.WriteEndElement(); // UpdateShopper

      return sbRequest.ToString();
    }


    #endregion

  }
}
