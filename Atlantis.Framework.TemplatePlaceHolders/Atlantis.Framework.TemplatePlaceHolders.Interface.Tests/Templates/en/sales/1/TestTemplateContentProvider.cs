
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface.Tests.Templates.en.sales._1
{
  public class TestTemplateContentProvider : ProviderBase, ITemplateContentProvider
  {
    public TestTemplateContentProvider(IProviderContainer container) : base(container)
    {
    }

    public string Content
    {
      get { return "<h1>@Model.Title</h1><div>@Model.Content</div>"; }
    }
  }
}
