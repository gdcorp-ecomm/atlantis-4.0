using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface.Tests
{
  public class TestTemplateDataSoureProvider : ProviderBase, ITemplateDataSourceProvider
  {
    public TestTemplateDataSoureProvider(IProviderContainer container) : base(container)
    {
    }

    public dynamic GetDataSource(IDataSource dataSourceOptions)
    {
      return new TestTemplateModel(dataSourceOptions.GetDataSource("us"));
    }
  }
}
