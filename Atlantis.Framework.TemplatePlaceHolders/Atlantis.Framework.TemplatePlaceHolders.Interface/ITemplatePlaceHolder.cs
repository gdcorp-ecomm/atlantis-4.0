
namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  public interface ITemplatePlaceHolder
  {
    string PlaceHolder { get; }

    ITemplateSource TemplateSource { get; }

    IDataSource DataSource { get; }
  }
}
