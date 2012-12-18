using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiPurchase.Interface
{
  [Serializable]
  public class BonsaiPurchaseResponseData : IResponseData
  {
    public BonsaiPurchaseResponseData()
    {
    }

    public bool IsSuccess { get; set; }
    public string OrderXml { get; set; }
    
    public string XML { get; set; }
    public string ToXML()
    {
      return XML;
    }

    public AtlantisException AtlException { get; set; }
    public AtlantisException GetException()
    {
      return AtlException;
    }

    public int BasketResultCode { get; set; }
  }
}
