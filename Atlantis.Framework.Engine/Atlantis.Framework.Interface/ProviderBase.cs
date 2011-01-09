
namespace Atlantis.Framework.Interface
{
  public abstract class ProviderBase
  {
    public IProviderContainer Container { get; private set; }

    protected ProviderBase(IProviderContainer container)
    {
      Container = container;
    }
  }
}
