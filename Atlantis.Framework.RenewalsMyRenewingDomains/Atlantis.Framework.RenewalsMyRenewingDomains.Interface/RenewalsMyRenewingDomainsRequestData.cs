using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RenewalsMyRenewingDomains.Interface
{
  public class RenewalsMyRenewingDomainsRequestData : RequestData
  {
    private int _daysfromexpiration = 90;
    public int DaysFromExpiration
    {
      get
      {
        return _daysfromexpiration;
      }
      set
      {
        if (value > 0)
        {
          _daysfromexpiration = value;
        }
      }
    }

    public RenewalsMyRenewingDomainsRequestData(
      string shopperID,
      string sourceURL,
      string orderID,
      string pathway,
      int pageCount)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public RenewalsMyRenewingDomainsRequestData(
      string shopperID,
      string sourceURL,
      string orderID,
      string pathway,
      int pageCount, int daysFromExpiration)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      DaysFromExpiration = daysFromExpiration;
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      return string.Empty;
    }

    #endregion
  }
}
