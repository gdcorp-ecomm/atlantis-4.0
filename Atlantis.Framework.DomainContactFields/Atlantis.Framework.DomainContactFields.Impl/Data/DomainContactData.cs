using System.Reflection;
using System.IO;

namespace Atlantis.Framework.DomainContactFields.Impl.Data
{
  internal static class DomainContactData
  {
    public static string DomainContactFieldsXml { get; private set; }

    static DomainContactData()
    {
      const string resourcePath = "Atlantis.Framework.DomainContactFields.Impl.Data.DomainContactFields.xml";
      Assembly assembly = Assembly.GetExecutingAssembly();
      using (var streamReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
      {
        DomainContactFieldsXml = streamReader.ReadToEnd();
      }
    }
  }
}
