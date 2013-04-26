namespace Atlantis.Framework.Render.Pipeline.Interface
{
  internal class ProcessedRenderContent : IProcessedRenderContent
  {
    public string Content { get; internal set; }
    
    internal ProcessedRenderContent(IRenderContent renderContent)
    {
      Content = string.Copy(renderContent.Content);
    }

    public void OverWriteContent(string content)
    {
      Content = content;
    }
  }
}
