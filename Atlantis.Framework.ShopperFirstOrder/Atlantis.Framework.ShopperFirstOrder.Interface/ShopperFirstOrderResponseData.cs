using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.ShopperFirstOrder.Interface
{
  public class ShopperFirstOrderResponseData : IResponseData
  {
    string _firstOrderId;
    AtlantisException _exception = null;

    public ShopperFirstOrderResponseData()
    {
    }

    private ShopperFirstOrderResponseData(string firstOrderId)
    {
      _firstOrderId = firstOrderId;
    }

    private ShopperFirstOrderResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public static IResponseData FromFirstOrderId(string firstOrderId)
    {
      return new ShopperFirstOrderResponseData(firstOrderId);
    }

    public static IResponseData FromAtlantisException(AtlantisException exception)
    {
      return new ShopperFirstOrderResponseData(exception);
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public bool HasOrder 
    {
      get { return !string.IsNullOrEmpty(_firstOrderId); } 
    }

    public string FirstOrderId 
    {
      get { return _firstOrderId; }
    }

  }
}
