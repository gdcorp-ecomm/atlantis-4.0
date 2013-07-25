using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;

namespace Atlantis.Framework.Providers.Geo
{
  public class GeoState : IGeoState
  {
    internal static IGeoState FromState(IProviderContainer container, State state)
    {
      return new GeoState(container, state);
    }

    private IProviderContainer _container;
    private State _state;

    private GeoState(IProviderContainer container, State state)
    {
      _container = container;
      _state = state;
    }

    public int Id
    {
      get { return _state.Id; }
    }

    public string Code
    {
      get { return _state.Code; }
    }

    public string Name
    {
      get { return _state.Name; }
    }
  }
}
