
namespace Atlantis.Framework.Providers.RenderPipeline.Interface
{
  public interface IProcessedRenderContent : IRenderContent
  {
    void OverWriteContent(string content);
  }
}
