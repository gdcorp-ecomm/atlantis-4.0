using Atlantis.Framework.Interface;
using System.Web;

namespace Atlantis.Framework.Providers.Personalization
{
  internal class ShopperSpecificSessionDataItem<TOutput>
  {
    private string _sessionDataKey;
    private string _sessionDataBelongsToKey;

    internal ShopperSpecificSessionDataItem(string sessionKey)
    {
      _sessionDataKey = sessionKey + ".Data";
      _sessionDataBelongsToKey = sessionKey + ".DataBelongsTo";
    }

    public bool TryGetData(RequestData belongsTo, out TOutput output)
    {
      if (TryGetDataFromSession(belongsTo, out output))
      {
        return true;
      }

      return false;
    }

    private bool TryGetDataFromSession(RequestData belongsTo, out TOutput output)
    {
      output = default(TOutput);

      string sessionBelongsTo = (string)SafeSession.GetSessionItem(_sessionDataBelongsToKey);
      if (sessionBelongsTo == null)
      {
        return false;
      }

      if (sessionBelongsTo != belongsTo.GetCacheMD5())
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
        output = (TOutput)rawData;
      }
      catch
      {
        return false;
      }

      return true;
    }

    public void SetData(RequestData belongsTo, TOutput output)
    {
      SetDataIntoSession(belongsTo, output);
    }

    private void SetDataIntoSession(RequestData belongsTo, TOutput data)
    {
      SafeSession.SetSessionItem(_sessionDataBelongsToKey, belongsTo.GetCacheMD5());
      SafeSession.SetSessionItem(_sessionDataKey, data);
    }
  }
}
