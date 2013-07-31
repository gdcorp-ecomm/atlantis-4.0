using System.IO;
using System.Reflection;

namespace Atlantis.Framework.Brand.Impl.Data
{
  internal class ProductLineNamesData
  {
    public static string ProductLineNamesXml { get; private set; }

    static ProductLineNamesData()
    {
      string xmlPath = "Atlantis.Framework.Brand.Impl.Data.ProductLineNames.xml";
      Assembly assembly = Assembly.GetExecutingAssembly();

      using (StreamReader textReader = new StreamReader(assembly.GetManifestResourceStream(xmlPath)))
      {
        ProductLineNamesXml = textReader.ReadToEnd();
      }
    }
  }
}