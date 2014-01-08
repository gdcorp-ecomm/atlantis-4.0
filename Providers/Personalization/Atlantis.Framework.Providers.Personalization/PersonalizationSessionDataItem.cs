using System.Web;

namespace Atlantis.Framework.Providers.Personalization
{
  internal class ShopperSpecificSessionDataItem<TInput, TOutput>
  {
    private string _sessionDataKey;
    private string _sessionDataBelongsToKey;

    internal ShopperSpecificSessionDataItem(string sessionKey)
    {
      _sessionDataKey = sessionKey + ".Data";
      _sessionDataBelongsToKey = sessionKey + ".DataBelongsTo";
    }

    public bool TryGetData(TInput belongsTo, out TOutput output)
    {
      if (TryGetDataFromSession(belongsTo, out output))
      {
        return true;
      }

      return false;
    }

    private bool TryGetDataFromSession(TInput belongsTo, out TOutput output)
    {
      output = default(TOutput);

      TInput sessionBelongsTo = (TInput)SafeSession.GetSessionItem(_sessionDataBelongsToKey);
      if (sessionBelongsTo == null)
      {
        return false;
      }

      if (!sessionBelongsTo.Equals(belongsTo))
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

    public void SetData(TInput belongsTo, TOutput output)
    {
      SetDataIntoSession(belongsTo, output);
    }

    private void SetDataIntoSession(TInput belongsTo, TOutput data)
    {
      SafeSession.SetSessionItem(_sessionDataBelongsToKey, belongsTo);
      SafeSession.SetSessionItem(_sessionDataKey, data);
    }
  }
}
