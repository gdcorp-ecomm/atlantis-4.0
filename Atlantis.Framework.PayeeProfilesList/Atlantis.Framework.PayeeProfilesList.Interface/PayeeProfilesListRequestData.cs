using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PayeeProfilesList.Interface
{
  public class PayeeProfilesListRequestData : RequestData
  {

    public PayeeProfilesListRequestData(string shopperId)
    {
      ShopperID = shopperId;
      RequestTimeout = TimeSpan.FromSeconds(50);
    }

  }
}
