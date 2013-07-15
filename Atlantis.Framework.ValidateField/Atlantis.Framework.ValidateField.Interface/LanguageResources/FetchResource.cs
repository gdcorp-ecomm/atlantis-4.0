using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.Caching;

namespace Atlantis.Framework.ValidateField.Interface.LanguageResources
{
  public class FetchResource : IDisposable
  {
    private readonly string _resourceNamespaceAndPrefix = string.Empty;
    private readonly string _defaultCulture = string.Empty;
    private readonly string _cultureToRetrieve = string.Empty;

    private ResourceManager _resourceManager;
    private ResourceManager ResourceManager
    {
      get
      {
        if (_resourceManager == null || !_resourceManager.BaseName.Equals(_resourceNamespaceAndPrefix, StringComparison.OrdinalIgnoreCase))
        {
          object resourceCacheObject = MemoryCache.Default[_resourceNamespaceAndPrefix];
          if (resourceCacheObject == null)
          {
            _resourceManager = new ResourceManager(_resourceNamespaceAndPrefix, Assembly.GetExecutingAssembly());
            MemoryCache.Default.Add(_resourceNamespaceAndPrefix, _resourceManager, DateTime.Now.AddMinutes(10));
          }
          else
          {
            _resourceManager = (ResourceManager)resourceCacheObject;
          }
        }

        return _resourceManager;
      }
    }

    //Atlantis.Framework.ShopperValidator.Interface.LanguageResources.ShopperValidator  is the resouceNamespaceAndPrefix for a file called "ShopperValidator.en.resx"
    //located in A.F.ShopperValidator.Interface\LanguageResources
    public FetchResource(string resourceNamespaceAndPrefix, string cultureToRetrieve, string defaultCulture = "en")
    {
      _resourceNamespaceAndPrefix = resourceNamespaceAndPrefix;
      _defaultCulture = defaultCulture;
      _cultureToRetrieve = cultureToRetrieve;
    }

    public string GetString(string keyName)
    {
      string keyValue = string.Empty;

      if (!TryGetString(keyName, _cultureToRetrieve, out keyValue)) //.net automatically falls back to neutral culture.  I.E de-DE will fallback to de
      {
        TryGetString(keyName, _defaultCulture, out keyValue);
      }

      return keyValue;
    }

    private bool TryGetString(string keyName, string culture, out string value)
    {
      bool stringFound = false;
      value = string.Empty;

      try
      {
        value = ResourceManager.GetString(keyName, new CultureInfo(culture));
        stringFound = true;
      }
      catch (Exception ex)
      { }

      return stringFound;
    }

    public void Dispose()
    {
      ResourceManager.ReleaseAllResources();
      MemoryCache.Default.Remove(_resourceNamespaceAndPrefix);
    }
  }
}
