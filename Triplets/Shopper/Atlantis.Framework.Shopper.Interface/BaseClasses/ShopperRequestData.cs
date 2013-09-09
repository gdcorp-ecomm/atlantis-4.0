using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Shopper.Interface.BaseClasses
{
  public abstract class ShopperRequestData : RequestData
  {
    private string _originIpAddress;
    private string _requestedBy;

    public ShopperRequestData()
    {
      _originIpAddress = Environment.MachineName;
      _requestedBy = Environment.MachineName;
    }

    public string OriginIpAddress 
    { 
      get
      {
        return _originIpAddress;
      }

      protected set
      {
        if (!string.IsNullOrEmpty(value))
        {
          _originIpAddress = value;
        }
      }
    }

    public string RequestedBy
    {
      get
      {
        return _requestedBy;
      }

      protected set
      {
        if (!string.IsNullOrEmpty(value))
        {
          _requestedBy = value;
        }
      }
    }
  }
}
