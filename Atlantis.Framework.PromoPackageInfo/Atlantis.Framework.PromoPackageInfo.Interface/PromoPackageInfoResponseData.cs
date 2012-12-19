using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoPackageInfo.Interface
{
  public class PromoPackageInfoResponseData : IResponseData
  {
    private AtlantisException _exception { get; set; }
    private List<PackageItem> _products;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public List<PackageItem> ProductList
    {
      get
      {
        if (_products == null)
        {
          _products = new List<PackageItem>();
        }
        return _products;
      }
      private set { _products = value; }
    }

    public PromoPackageInfoResponseData(List<PackageItem> products)
    {
      if (products == null)
        throw new ArgumentNullException("products", "products is null.");

      ProductList = products;
    }

    public PromoPackageInfoResponseData(AtlantisException aex)
    {
      _exception = aex;
    }

    public PromoPackageInfoResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message, ex.StackTrace, ex);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return string.Empty;
    }
  }
}