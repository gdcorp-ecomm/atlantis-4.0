
namespace Atlantis.Framework.Render.Pipeline.Interface
{
  public interface IProcessedRenderContent : IRenderContent
  {
    void OverWriteContent(string content);
  }
}
