using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace Atlantis.Framework.ValidateInput.Impl.Data
{
  internal static class FieldValidationData
  {
    private static Dictionary<string, string> _fieldValidations = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

    static FieldValidationData()
    {
      LoadFieldValidation("password");
    }

    private static void LoadFieldValidation(string key)
    {
      string resourcePath = "Atlantis.Framework.ValidateInput.Impl.Data." + key + ".xml";
      var assembly = Assembly.GetExecutingAssembly();
      using (var textReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
      {
        string xml = textReader.ReadToEnd();
        _fieldValidations[key] = xml;
      }
    }

    public static bool TryGetFieldValidationXml(string key, out string xml)
    {
      return _fieldValidations.TryGetValue(key, out xml);
    }
  }
}