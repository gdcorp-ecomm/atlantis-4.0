using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaProduct.Interface;

namespace Atlantis.Framework.MyaProductPremiumDNS.Interface
{
  public class MyaProductPremiumDNSResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;
    public IList<PremiumDNSProduct> PremiumDNSProducts { get; private set; }
    public IPagingResult PagingResult { get; private set; }
    public bool IsSuccess
    {
      get { return _atlantisException == null; }
    }

    public MyaProductPremiumDNSResponseData(IList<PremiumDNSProduct> premiumDNSProducts, int totalRecords, int totalPages)
    {
      PremiumDNSProducts = premiumDNSProducts;
      PagingResult = new PremiumDNSPagingResult(totalRecords, totalPages);
    }

    public MyaProductPremiumDNSResponseData(AtlantisException atlantisException)
    {
      PremiumDNSProducts = new List<PremiumDNSProduct>(1);
      PagingResult = new PremiumDNSPagingResult(0, 0);
      _atlantisException = atlantisException;
    }

    public MyaProductPremiumDNSResponseData(RequestData requestData, Exception ex)
    {
      PremiumDNSProducts = new List<PremiumDNSProduct>(1);
      PagingResult = new PremiumDNSPagingResult(0, 0);
      _atlantisException = new AtlantisException(requestData
        , MethodBase.GetCurrentMethod().DeclaringType.FullName
        , string.Format("MyaProductPremiumDNS Error: {0}", ex.Message)
        , ex.Data.ToString()
        , ex);            
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion

  }
}
