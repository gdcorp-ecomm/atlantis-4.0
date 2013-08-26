using System.Globalization;
using System.Web;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Shopper.Interface
{
  public class GetShopperRequestData : RequestData
  {
    public string RequestedBy { get; private set; }

    private readonly string _ipAddress;
    private readonly HashSet<int> _communicationPreferences;
    private readonly List<InterestPreference> _interestPreferences;
    private readonly HashSet<string> _fields;

    public GetShopperRequestData(string shopperId, string originIpAddress, string requestedBy, IEnumerable<string> fields = null)
    {
      ShopperID = shopperId;
      _ipAddress = originIpAddress;
      RequestedBy = requestedBy;

      _communicationPreferences = new HashSet<int>();
      _interestPreferences = new List<InterestPreference>();

      _fields = fields != null ? 
        new HashSet<string>(fields) : 
        new HashSet<string>();
    }

    public IEnumerable<string> Fields
    {
      get
      {
        return _fields;
      }
    }

    public IEnumerable<int> CommunicationPreferences
    {
      get { return _communicationPreferences; }
    }

    public IEnumerable<InterestPreference> InterestPreferences
    {
      get { return _interestPreferences; }
    }

    public void AddField(string field)
    {
      if (!string.IsNullOrEmpty(field))
      {
        _fields.Add(field);
      }
    }

    public void AddFields(IEnumerable<string> fields)
    {
      if (fields != null)
      {
        _fields.UnionWith(fields);
      }
    }

    public void AddCommunicationPreference(int communicationTypeId)
    {
      _communicationPreferences.Add(communicationTypeId);
    }

    public void AddInterestPreference(InterestPreference preference)
    {
      if (preference != null)
      {
        _interestPreferences.Add(preference);
      }
    }

    public override string ToXML()
    {
      var requestElement = new XElement("ShopperGet");
      requestElement.Add(new XAttribute("ID", ShopperID));
      requestElement.Add(new XAttribute("IPAddress", GetIPAddress()));
      requestElement.Add(new XAttribute("RequestedBy", RequestedBy));

      var fieldsElement = new XElement("ReturnFields");
      requestElement.Add(fieldsElement);

      foreach (var field in _fields)
      {
        fieldsElement.Add(new XElement("Field", new XAttribute("Name", field)));
      }

      var preferencesElement = new XElement("ReturnPreferences");
      requestElement.Add(preferencesElement);

      foreach (var interestPreference in _interestPreferences)
      {
        preferencesElement.Add(new XElement("Interest", 
          new XAttribute("CommTypeID", interestPreference.CommunicationTypeId.ToString(CultureInfo.InvariantCulture)),
          new XAttribute("InterestTypeID", interestPreference.InterestTypeId.ToString(CultureInfo.InvariantCulture))));
      }

      foreach (var communicationPreference in _communicationPreferences)
      {
        preferencesElement.Add(new XElement("Communication",
          new XAttribute("CommTypeID", communicationPreference.ToString(CultureInfo.InvariantCulture))));
      }

      return requestElement.ToString(SaveOptions.DisableFormatting);
    }

    private object GetIPAddress()
    {
      return string.IsNullOrEmpty(_ipAddress) ? Environment.MachineName : _ipAddress;
    }
  }
}
