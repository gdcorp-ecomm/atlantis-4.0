using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Security.Cryptography;

namespace Atlantis.Framework.EcommDomainFreeProductCredit.Interface
{
  public class EcommDomainFreeProductCreditRequestData : RequestData
  {
    public EcommDomainFreeProductCreditRequestData(
      string shopperId,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount,
      int productTypeID)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _productTypeID = productTypeID;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    private int _productTypeID;

    public int ProductTypeID
    {
      get { return _productTypeID; }
      set { _productTypeID = value; }
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
