using Atlantis.Framework.Shopper.Interface.BaseClasses;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Interface
{
  public class SearchShoppersRequestData : ShopperRequestData
  {
    Dictionary<string, string> _searchFields;
    HashSet<string> _returnFields;

    public SearchShoppersRequestData(string originIpAddress, string requestedBy, IDictionary<string, string> searchFields, IEnumerable<string> returnFields = null)
    {
      OriginIpAddress = originIpAddress;
      RequestedBy = requestedBy;

      if ((searchFields == null) || (searchFields.Count == 0))
      {
        throw new ArgumentException("At least 1 search field is required.");
      }

      _searchFields = new Dictionary<string, string>();
      AddSearchFields(searchFields);

      if (_searchFields.Count == 0)
      {
        throw new ArgumentException("At least 1 valid non-null search field is required.");
      }

      _returnFields = new HashSet<string>();
      _returnFields.Add("shopper_id");
      AddReturnFields(returnFields);
    }

    private void AddSearchFields(IDictionary<string, string> searchFields)
    {
      foreach (var field in searchFields)
      {
        if (!string.IsNullOrEmpty(field.Key) && (field.Value != null))
        {
          _searchFields[field.Key] = field.Value;
        }
      }
    }

    public override string ToXML()
    {
      var shopperSearchElement = new XElement("ShopperSearch");
      shopperSearchElement.Add(
        new XAttribute("IPAddress", OriginIpAddress),
        new XAttribute("RequestedBy", RequestedBy));

      var searchFieldsElement = new XElement("SearchFields");
      shopperSearchElement.Add(searchFieldsElement);

      if (_searchFields.Count > 0)
      {
        foreach (var searchField in _searchFields)
        {
          if (!string.IsNullOrEmpty(searchField.Key))
          {
            var fieldElement = new XElement("Field");
            fieldElement.Add(new XAttribute("Name", searchField.Key));
            fieldElement.Value = searchField.Value;
            searchFieldsElement.Add(fieldElement);
          }
        }
      }

      var returnFieldsElement = new XElement("ReturnFields");
      shopperSearchElement.Add(returnFieldsElement);

      if (_returnFields.Count > 0)
      {
        foreach (var field in _returnFields)
        {
          returnFieldsElement.Add(new XElement("Field", new XAttribute("Name", field)));
        }
      }

      return shopperSearchElement.ToString(SaveOptions.DisableFormatting);
    }

    private void AddReturnFields(IEnumerable<string> returnFields)
    {
      if (returnFields != null)
      {
        _returnFields.UnionWith(returnFields);
      }
    }
  }
}
