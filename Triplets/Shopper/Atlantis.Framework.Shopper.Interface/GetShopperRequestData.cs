using Atlantis.Framework.Shopper.Interface.BaseClasses;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Interface
{
  public class GetShopperRequestData : ShopperRequestData
  {
    private readonly HashSet<string> _fields;

    public GetShopperRequestData(string shopperId, string originIpAddress, string requestedBy, IEnumerable<string> fields = null)
    {
      ShopperID = shopperId;
      OriginIpAddress = originIpAddress;
      RequestedBy = requestedBy;

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

    public override string ToXML()
    {
      var requestElement = new XElement("ShopperGet");
      requestElement.Add(new XAttribute("ID", ShopperID));
      requestElement.Add(new XAttribute("IPAddress", OriginIpAddress));
      requestElement.Add(new XAttribute("RequestedBy", RequestedBy));

      var fieldsElement = new XElement("ReturnFields");
      requestElement.Add(fieldsElement);

      foreach (var field in _fields)
      {
        fieldsElement.Add(new XElement("Field", new XAttribute("Name", field)));
      }

      return requestElement.ToString(SaveOptions.DisableFormatting);
    }

  }
}
