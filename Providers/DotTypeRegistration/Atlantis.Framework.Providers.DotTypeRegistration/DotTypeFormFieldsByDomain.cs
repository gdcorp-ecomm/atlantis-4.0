using System;
using System.Collections.Generic;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Newtonsoft.Json;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration
{
  public class DotTypeFormFieldsByDomain : IDotTypeFormFieldsByDomain
  {
    public IDictionary<string, IList<IList<IFormField>>> FormFieldsByDomain { get; set; }
    public IFormItems FormItems { get; set; }
    public string ToJson { get; set; }

    public DotTypeFormFieldsByDomain(IDictionary<string, IList<IList<IFormField>>> formFieldsByDomain, IFormItems formItems)
    {
      FormFieldsByDomain = formFieldsByDomain;
      FormItems = formItems;
      ToJson = ConvertFormFieldsToJson();
    }

    private string ConvertFormFieldsToJson()
    {
      var result = string.Empty;
      try
      {
        result = JsonConvert.SerializeObject(this);
        result = result.Replace(",\"ToJson\":null", "");
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("DotTypeFormFieldsByDomain.ConvertFormFieldsToJson", 0, ex.ToString(), FormFieldsByDomain.ToString());
        Engine.Engine.LogAtlantisException(aex);
      }

      return result;
    }
  }
}
