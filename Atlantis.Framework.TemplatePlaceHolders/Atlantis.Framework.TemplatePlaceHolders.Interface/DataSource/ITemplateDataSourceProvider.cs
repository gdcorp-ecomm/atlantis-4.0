
namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  public interface ITemplateDataSourceProvider
  {
    dynamic GetDataSource(IDataSource dataSourceOptions);
  }
}
