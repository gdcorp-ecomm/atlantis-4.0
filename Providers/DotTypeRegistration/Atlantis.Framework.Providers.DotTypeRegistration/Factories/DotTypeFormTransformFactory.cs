using Atlantis.Framework.Providers.DotTypeRegistration.Handlers;
using Atlantis.Framework.Providers.DotTypeRegistration.Handlers.MobileDefault;
using Atlantis.Framework.Providers.DotTypeRegistration.Handlers.MobileRich;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Factories
{
  public static class DotTypeFormTransformFactory
  {
    public static IDotTypeFormTransformHandler GetFormTransformHandler(ViewTypes viewType)
    {
      IDotTypeFormTransformHandler formTransform = null;

      switch (viewType)
      {
        case ViewTypes.MobileRich:
          formTransform = new MobileRichFormTransformHandler();
          break;
        case ViewTypes.MobileDefault:
          formTransform = new MobileDefaultFormTransformHandler();
          break;
      }

      return formTransform;
    }
  }
}
