
namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal interface IRenderingEngine
  {
    string Render(string rawContent, dynamic model);
  }
}
