using System.IO;
using System.Reflection;

namespace Atlantis.Framework.Brand.Impl.Data
{
  internal class CompanyNamesData
  {
    public static string CompanyNamesXml { get; private set; }

    static CompanyNamesData()
    {
      string xmlPath = "Atlantis.Framework.Brand.Impl.Data.CompanyNames.xml";
      Assembly assembly = Assembly.GetExecutingAssembly();

      using (StreamReader textReader = new StreamReader(assembly.GetManifestResourceStream(xmlPath)))
      {
        CompanyNamesXml = textReader.ReadToEnd();
      }
    }
  }
}