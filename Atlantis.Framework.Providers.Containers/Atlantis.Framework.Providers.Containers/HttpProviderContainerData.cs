using System.Web;

namespace Atlantis.Framework.Providers.Containers
{
  internal class HttpProviderContainerData : ProviderContainerDataBase
  {
    private const string KEY_FORMAT = "Atlantis.Framework.Interface.HttpProviderContainerData.{0}";

    private string GetFormattedKey(string key)
    {
      return string.Format(KEY_FORMAT, key);
    }

    protected override void SetContextData(string key, object value)
    {
      if (HttpContext.Current != null)
      {
        HttpContext.Current.Items[GetFormattedKey(key)] = value;
      }
    }

    protected override object GetContextData(string key)
    {
      object value = null;

      if (HttpContext.Current != null)
      {
        value = HttpContext.Current.Items[GetFormattedKey(key)];
      }

      return value;
    }
  }
}