
namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal interface IRenderingEngine
  {
    string Render<T>(string rawContent, T model);
  }
}
