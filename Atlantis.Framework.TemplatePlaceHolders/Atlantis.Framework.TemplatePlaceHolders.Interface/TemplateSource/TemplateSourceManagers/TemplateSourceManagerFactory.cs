
namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal static class TemplateSourceManagerFactory
  {
    internal static ITemplateSourceManager GetInstance(string source)
    {
      ITemplateSourceManager templateSourceManager;

      switch(source.ToLowerInvariant())
      {
        case "cds":
          templateSourceManager = new CdsTemplateSourceManager();
          break;
        case "localweb":
          templateSourceManager = new LocalWebTemplateSourceManager();
          break;
        case "codeclass":
          templateSourceManager = new CodeClassTemplateSourceManager();
          break;
        default:
          templateSourceManager = new NullTemplateSourceManager();
          break;
      }

      return templateSourceManager;
    }
  }
}
