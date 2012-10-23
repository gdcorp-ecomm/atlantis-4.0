
namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal class NullRenderingEngine : IRenderingEngine
  {
    public string Render<T>(string rawContent, T model)
    {
      // TODO: Log silent?
      return string.Empty;
    }
  }
}
