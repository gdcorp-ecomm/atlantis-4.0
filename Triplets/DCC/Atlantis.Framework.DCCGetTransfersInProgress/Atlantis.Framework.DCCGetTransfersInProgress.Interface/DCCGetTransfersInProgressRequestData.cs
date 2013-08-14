using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetTransfersInProgress.Interface
{
  public class DCCGetTransfersInProgressRequestData : RequestData
  {
    public int Quantity { get; private set; }
    public string UserName { get; private set; }

    public DCCGetTransfersInProgressRequestData(string shopperId, int quantity, string userName)
    {
      ShopperID = shopperId;
      Quantity = quantity;
      UserName = userName;
    }

    public override string ToXML()
    {
      var d = new XDocument(
        new XElement("getdcctransfersinprogress",
            new XElement("username", UserName),
            new XElement("search",
                new XAttribute("shopperid", ShopperID),
                new XAttribute("quantity", Quantity.ToString()),
                new XAttribute("direction", "ASC")
                )
            )
        );
      return d.ToString();
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in DCCGetTransfersInProgressRequestData");
    }

  }
}
