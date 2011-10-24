using System;
using Atlantis.Framework.Interface;

namespace MobilePushShopperAdd.Interface
{
  public class MobilePushShopperAddResponseData : IResponseData
  {
    private AtlantisException AtlantisException { get; set; }

    public string Xml { get; set; }

    public MobilePushShopperAddResponseData(string responseXml)
    {
      
    }

    public MobilePushShopperAddResponseData(MobilePushShopperAddRequestData requestData, Exception ex)
    {

    }

    public string ToXML()
    {
      return Xml;
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }
  }
}
