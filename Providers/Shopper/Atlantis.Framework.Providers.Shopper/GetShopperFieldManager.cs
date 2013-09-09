using Atlantis.Framework.Shopper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.Shopper
{
  internal class GetShopperFieldManager
  {
    private HashSet<string> _registeredFields;
    private Dictionary<string, HashSet<string>> _receivedFieldsByShopperId;

    internal GetShopperFieldManager()
    {
      _registeredFields = new HashSet<string>();
      _receivedFieldsByShopperId = new Dictionary<string, HashSet<string>>();
    }

    public void RegisterNeededField(string fieldName)
    {
      _registeredFields.Add(fieldName);
    }

    public void RegisterNeededFields(IEnumerable<string> neededFields)
    {
      _registeredFields.UnionWith(neededFields);
    }

    public void RegisterResponseFields(GetShopperResponseData response)
    {
      if (string.IsNullOrEmpty(response.ShopperId))
      {
        return;
      }

      HashSet<string> fields;
      if (!_receivedFieldsByShopperId.TryGetValue(response.ShopperId, out fields))
      {
        fields = new HashSet<string>();
        _receivedFieldsByShopperId[response.ShopperId] = fields;
      }

      fields.UnionWith(response.Fields);
    }

    public HashSet<string> GetNeededFieldsForShopper(string shopperId)
    {
      HashSet<string> result = new HashSet<string>(_registeredFields);

      HashSet<string> alreadyReceivedFields;
      if ((!string.IsNullOrEmpty(shopperId)) && (_receivedFieldsByShopperId.TryGetValue(shopperId, out alreadyReceivedFields)))
      {
        result.ExceptWith(alreadyReceivedFields);
      }

      return result;
    }

    public void ClearShopper(string shopperId)
    {
      _receivedFieldsByShopperId.Remove(shopperId);
    }
  }
}
