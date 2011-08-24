using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyerProfileGetByShopperID.Interface
{
  public class BuyerProfileGetByShopperIDResponseData : IResponseData
  {
    private AtlantisException _atlException = null;
    private bool _success = false;
    private List<ProfileSummary> _profiles;

    public BuyerProfileGetByShopperIDResponseData(List<ProfileSummary> profiles)
    {
      _success = true;
      _profiles = profiles;
    }

    public BuyerProfileGetByShopperIDResponseData(RequestData oRequestData, Exception ex)
    {
      _success = false;
      _atlException = new AtlantisException(oRequestData, "BuyerProfileGetByShopperIDResponseData", ex.Message, string.Empty);
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public List<ProfileSummary> GetProfiles
    {
      get
      {
        return _profiles;
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    #endregion

    public AtlantisException GetException()
    {
      return _atlException;
    }

  }
}
