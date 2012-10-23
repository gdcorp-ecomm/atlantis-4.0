using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  public interface ITemplatePlaceHolderEvaluator
  {
    string EvaluatePlaceHolder(ITemplatePlaceHolder templatePlaceHolder, IProviderContainer providerContainer);
  }
}
