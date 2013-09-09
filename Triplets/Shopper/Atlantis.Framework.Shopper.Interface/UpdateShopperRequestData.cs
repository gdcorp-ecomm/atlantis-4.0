using Atlantis.Framework.Shopper.Interface.BaseClasses;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Interface
{
  public class UpdateShopperRequestData : ShopperRequestData
  {
    Dictionary<string, string> _updateFields;

    public UpdateShopperRequestData(string shopperId, string originIpAddress, string requestedBy, IDictionary<string, string> updateFields = null)
    {
      ShopperID = shopperId;
      OriginIpAddress = originIpAddress;
      RequestedBy = requestedBy;

      if (updateFields == null)
      {
        _updateFields = new Dictionary<string, string>(10);
      }
      else
      {
        _updateFields = new Dictionary<string, string>(updateFields);
      }
    }

    public void AddFields(IDictionary<string, string> updateFields)
    {
      if (updateFields != null)
      {
        foreach (var field in updateFields)
        {
          AddField(field.Key, field.Value);
        }
      }
    }

    public void AddField(string fieldName, string fieldValue)
    {
      if (!string.IsNullOrEmpty(fieldName) && (fieldValue != null))
      {
        _updateFields[fieldName] = fieldValue;
      }
    }

    public override string ToXML()
    {
      var updateElement = new XElement("ShopperUpdate");
      updateElement.Add(
        new XAttribute("ID", ShopperID),
        new XAttribute("IPAddress", OriginIpAddress),
        new XAttribute("RequestedBy", RequestedBy));

      if (_updateFields.Count > 0)
      {
        var fieldsElement = new XElement("Fields");
        updateElement.Add(fieldsElement);

        foreach (var updateField in _updateFields)
        {
          if (!string.IsNullOrEmpty(updateField.Key))
          {
            var fieldElement = new XElement("Field");
            fieldElement.Add(new XAttribute("Name", updateField.Key));
            fieldElement.Value = updateField.Value ?? string.Empty;
            fieldsElement.Add(fieldElement);
          }
        }
      }

      return updateElement.ToString(SaveOptions.DisableFormatting);

    }

  }
}
