
namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  public interface ITemplateSource
  {
    string Format { get; }

    string Source { get; }

    string SourceAssembly { get; }

    string RequestKey { get; }
  }
}
