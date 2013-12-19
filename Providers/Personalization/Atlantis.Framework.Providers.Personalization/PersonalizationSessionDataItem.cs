using System.Web;

namespace Atlantis.Framework.Providers.Personalization
{
  internal class ShopperSpecificSessionDataItem<T>
  {
    private string _sessionDataKey;
    private string _sessionDataBelongsToKey;

    internal ShopperSpecificSessionDataItem(string sessionKey)
    {
      _sessionDataKey = sessionKey + ".Data";
      _sessionDataBelongsToKey = sessionKey + ".DataBelongsTo";
    }

    public bool TryGetData(string shopperId, string interactonPoint, string privateLabelId, out T data)
    {
      if (TryGetDataFromSession(shopperId, interactonPoint, privateLabelId, out data))
      {
        return true;
      }

      return false;
    }

    private bool TryGetDataFromSession(string shopperId, string interactonPoint, string privateLabelId, out T data)
    {
      data = default(T);

      string sessionBelongsTo = SafeSession.GetSessionItem(_sessionDataBelongsToKey) as string;
      if (sessionBelongsTo == null)
      {
        return false;
      }

      if (sessionBelongsTo != GetBelongsToValue(shopperId, interactonPoint, privateLabelId))
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

      return true;
    }

    public void SetData(string shopperId, string interactonPoint, string privateLabelId, T data)
    {
      SetDataIntoSession(shopperId, interactonPoint, privateLabelId, data);
    }

    private void SetDataIntoSession(string shopperId, string interactonPoint, string privateLabelId, T data)
    {
      SafeSession.SetSessionItem(_sessionDataBelongsToKey, GetBelongsToValue(shopperId, interactonPoint, privateLabelId));
      SafeSession.SetSessionItem(_sessionDataKey, data);
    }

    private string GetBelongsToValue(string shopperId, string interactonPoint, string privateLabelId)
    {
      return string.Format("{0}_{1}_{2}", shopperId ?? string.Empty, interactonPoint, privateLabelId);
    }
  }
}
