using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetTransfersInProgress.Interface
{
  public class DCCGetTransfersInProgressRequestData : RequestData
  {

    public int Quantity { get; set; }
    public string UserName { get; set; }

    public DCCGetTransfersInProgressRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int quantity, string userName)
      :base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Quantity = quantity;
      UserName = userName;
    }

    public override string ToXML()
    {
      XDocument d = new XDocument(
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
