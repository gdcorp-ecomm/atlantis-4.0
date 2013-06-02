
namespace Atlantis.Framework.Providers.Containers
{
  internal abstract class ProviderContainerDataBase
  {
    protected abstract object GetContextData(string key);

    protected abstract void SetContextData(string key, object value);

    internal T GetData<T>(string key, T defaultValue)
    {
      T value = defaultValue;

      object contextValue = GetContextData(key);

      if (contextValue != null)
      {
        try
        {
          value = (T)contextValue;
        }
        catch
        {
          value = defaultValue;
        }
      }

      return value;
    }

    internal void SetData<T>(string key, T value)
    {
      SetContextData(key, value);
    }
  }
}