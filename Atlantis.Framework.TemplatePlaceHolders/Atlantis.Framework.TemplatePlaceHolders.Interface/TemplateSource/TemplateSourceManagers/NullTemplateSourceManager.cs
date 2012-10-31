using System;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal class NullTemplateSourceManager : ITemplateSourceManager
  {
    public string GetTemplateSource(ITemplateSource templateSource, IProviderContainer providerContainer)
    {
      ErrorLogHelper.LogError(new Exception("NullTemplateSourceManager selected. Please verify you have a valid template source in your placeholder."), MethodBase.GetCurrentMethod().DeclaringType.FullName);
      return string.Empty;
    }
  }
}
