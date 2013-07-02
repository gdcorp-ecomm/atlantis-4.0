using Atlantis.Framework.Personalization.Interface;

namespace Atlantis.Framework.Providers.Personalization.Interface
{
  public interface IPersonalizationProvider
  {
    TargetedMessages GetTargetedMessages();
  }
}
