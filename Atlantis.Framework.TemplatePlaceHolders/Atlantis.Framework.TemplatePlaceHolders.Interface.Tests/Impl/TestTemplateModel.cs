
namespace Atlantis.Framework.TemplatePlaceHolders.Interface.Tests
{
  public class TestTemplateModel
  {
    public string Title { get { return "Title Text"; } }

    public string Content { get; private set; }

    public TestTemplateModel(string dataId)
    {
      Content = "Hello World - Data Source Option " + dataId;
    }
  }
}
