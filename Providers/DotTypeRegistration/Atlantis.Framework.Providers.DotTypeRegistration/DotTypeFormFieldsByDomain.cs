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
    public string ToJson { get; set; }

    public DotTypeFormFieldsByDomain(IDictionary<string, IList<IList<IFormField>>> formFieldsByDomain)
    {
      FormFieldsByDomain = formFieldsByDomain;
      ToJson = ConvertFormFieldsToJson();
    }

    private string ConvertFormFieldsToJson()
    {
      var result = string.Empty;
      try
      {
        result = JsonConvert.SerializeObject(FormFieldsByDomain);
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("DotTypeFormFieldsByDomain.ConvertFormFieldsToJson", "0", ex.ToString(), FormFieldsByDomain.ToString(), null, null);
        Engine.Engine.LogAtlantisException(aex);
      }

      return result;
    }
  }
}
