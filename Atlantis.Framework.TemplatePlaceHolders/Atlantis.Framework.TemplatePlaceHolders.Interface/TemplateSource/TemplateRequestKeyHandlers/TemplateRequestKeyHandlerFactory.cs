using System;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal static class TemplateRequestKeyHandlerFactory
  {
    internal static ITemplateRequestKeyHandlerProvider GetInstance(IProviderContainer providerContainer)
    {
      ITemplateRequestKeyHandlerProvider templateRequestKeyHandlerProvider;

      if (!providerContainer.TryResolve(out templateRequestKeyHandlerProvider))
      {
        ErrorLogHelper.LogError(new Exception("Cannot resolve type \"ITemplateRequestKeyHandlerProvider\".  Make sure it is registered by your provider container."), MethodBase.GetCurrentMethod().DeclaringType.FullName);
      }

      return templateRequestKeyHandlerProvider;
    }
  }
}
