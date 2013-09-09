using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Shopper.Interface.BaseClasses
{
  public abstract class ShopperResponseData : IResponseData
  {
    public ShopperResponseStatus Status { get; private set; }

    public ShopperResponseData(ShopperResponseStatus status)
    {
      Status = status;
    }

    public abstract string ToXML();

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
