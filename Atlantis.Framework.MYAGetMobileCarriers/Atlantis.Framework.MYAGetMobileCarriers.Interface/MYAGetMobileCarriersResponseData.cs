using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MYAGetMobileCarriers.Interface
{
  public class MYAGetMobileCarriersResponseData : IResponseData
  {
    private AtlantisException _atlException;

    public bool IsSuccess { get; private set;} 

    public List<MobileCarrierItem> CarrierList { get; private set; }
   
    public MYAGetMobileCarriersResponseData(List<MobileCarrierItem> carrierList)
    {
      IsSuccess = true;
      CarrierList = carrierList;
    }

    public MYAGetMobileCarriersResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      _atlException = new AtlantisException(oRequestData, "MYAGetMobileCarriersResponseData", ex.Message, string.Empty);
    }
    
    #region Implementation of IResponseData

    public string ToXML()
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<MobileCarrierItem>));
      StringWriter writer = new StringWriter();

      xmlSerializer.Serialize(writer, CarrierList);
      
      return writer.ToString();

    }

    public AtlantisException GetException()
    {
      return _atlException;
    }

    #endregion
  }
}
