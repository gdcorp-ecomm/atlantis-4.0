using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace Atlantis.Framework.ValidateField.Impl.Data
{
  internal static class Resources
  {
    static Dictionary<string, string> _fieldValidations = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

    static Resources()
    {
      LoadFieldValidation("password");
    }

    private static void LoadFieldValidation(string key)
    {
      string resourcePath = "Atlantis.Framework.ValidateField.Impl." + key + ".xml";
      Assembly assembly = Assembly.GetExecutingAssembly();
      using (StreamReader textReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
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
