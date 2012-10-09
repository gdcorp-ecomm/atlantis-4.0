using System.IO;
using System.Reflection;

namespace Atlantis.Framework.Tokens.Tests.Data
{
  internal static class TestData
  {
    public static string GetTextDataResource(string dataName)
    {
      string result;
      string resourcePath = "Atlantis.Framework.Tokens.Tests.Data." + dataName;
      Assembly assembly = Assembly.GetExecutingAssembly();
      using (StreamReader textReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
      {
        result = textReader.ReadToEnd();
      }

      return result;
    }
  }
}
