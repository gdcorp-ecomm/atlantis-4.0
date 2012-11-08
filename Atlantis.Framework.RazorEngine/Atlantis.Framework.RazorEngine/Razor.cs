using Atlantis.Framework.RazorEngine.Compilation.CSharp;
using Atlantis.Framework.RazorEngine.Cryptography;
using Atlantis.Framework.RazorEngine.Templating;

namespace Atlantis.Framework.RazorEngine
{
  public static class Razor
  {
    private static readonly TemplateService _templateService;

    static Razor()
    {
      _templateService = new TemplateService(new CSharpDirectCompilerService());
    }

    public static string CompileAndRun<T>(string template, T model, string saveLocation = null)
    {
      string cacheKey = Md5Helper.GenerateHash(template);
      return _templateService.CompileAndRun(cacheKey, template, model, saveLocation);
    }
  }
}