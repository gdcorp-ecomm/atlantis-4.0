using System.IO;
using System.Reflection;

namespace Atlantis.Framework.Providers.Localization
{
  internal class CountrySiteRedirectRuleData
  {
    public static string CountrySiteRedirectRuleXml { get; private set; }

    static CountrySiteRedirectRuleData()
    {
      const string resourcePath = "Atlantis.Framework.Providers.Localization.LocalizationRedirectProvider.CountrySiteRedirectRule.xml";
      var assembly = Assembly.GetExecutingAssembly();
     
        using (var streamReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
        {
          CountrySiteRedirectRuleXml = streamReader.ReadToEnd();
        }
      
    }
  }
}
