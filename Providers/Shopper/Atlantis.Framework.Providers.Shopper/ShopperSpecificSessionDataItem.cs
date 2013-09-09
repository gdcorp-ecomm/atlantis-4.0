using System.Web;

namespace Atlantis.Framework.Providers.Shopper
{
  internal class ShopperSpecificSessionDataItem<T>
  {
    private string _sessionDataKey;
    private string _sessionShopperKey;

    private string _instanceShopperId = null;
    private T _instanceData;

    internal ShopperSpecificSessionDataItem(string sessionKey)
    {
      _sessionShopperKey = sessionKey + ".ShopperId";
      _sessionDataKey = sessionKey + ".Data";
    }

    public bool TryGetData(string shopperId, out T data)
    {
      if (TryGetDataFromInstance(shopperId, out data))
      {
        return true;
      }

      if (TryGetDataFromSession(shopperId, out data))
      {
        return true;
      }

      return false;
    }

    private bool TryGetDataFromSession(string shopperId, out T data)
    {
      data = default(T);

      string sessionShopperId = SafeSession.GetSessionItem(_sessionShopperKey) as string;
      if (sessionShopperId == null)
      {
        return false;
      }

      if (sessionShopperId != shopperId)
      {
        return false;
      }

      object rawData = SafeSession.GetSessionItem(_sessionDataKey);
      if (rawData == null)
      {
        return false;
      }

      try
      {
        data = (T)rawData;
      }
      catch
      {
        return false;
      }

      SetDataIntoInstance(shopperId, data);
      return true;
    }

    private bool TryGetDataFromInstance(string shopperId, out T data)
    {
      data = default(T);

      if (_instanceShopperId == null)
      {
        return false;
      }

      if (_instanceShopperId != shopperId)
      {
        return false;
      }

      data = _instanceData;
      return true;
    }

    public void SetData(string shopperId, T data)
    {
      SetDataIntoInstance(shopperId, data);
      SetDataIntoSession(shopperId, data);
    }

    private void SetDataIntoSession(string shopperId, T data)
    {
      SafeSession.SetSessionItem(_sessionShopperKey, shopperId ?? string.Empty);
      SafeSession.SetSessionItem(_sessionDataKey, data);
    }

    private void SetDataIntoInstance(string shopperId, T data)
    {
      _instanceShopperId = shopperId;
      _instanceData = data;
    }
  }
}
