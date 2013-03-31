using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaMirageData.Interface;
using Atlantis.Framework.MyaMirageStatus.Interface;
using Atlantis.Framework.MyaRecentNamespaces.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.PurchaseEmail.Interface.Providers
{
  internal class ShopperProductProvider : ProviderBase
  {
    private Lazy<IShopperContext> _shopperContext;
    private Lazy<MyaMirageDataResponseData> _mirageData;
    private Lazy<MyaRecentNamespacesResponseData> _recentNamespaces;
    private Lazy<MyaMirageStatusResponseData> _mirageStatus;

    public ShopperProductProvider(IProviderContainer container)
      :base(container)
    {
      _shopperContext = new Lazy<IShopperContext>(() => { return Container.Resolve<IShopperContext>(); });
      _mirageData = new Lazy<MyaMirageDataResponseData>(() => { return LoadMirageData(); });
      _mirageStatus = new Lazy<MyaMirageStatusResponseData>(() => { return LoadMirageStatus(); });
      _recentNamespaces = new Lazy<MyaRecentNamespacesResponseData>(() => {return LoadRecentNamespaces();});
    }

    private MyaMirageDataResponseData LoadMirageData()
    {
      MyaMirageDataResponseData result = null;

      if (!string.IsNullOrEmpty(_shopperContext.Value.ShopperId))
      {
        try
        {
          MyaMirageDataRequestData request = new MyaMirageDataRequestData(_shopperContext.Value.ShopperId, string.Empty, string.Empty, string.Empty, 0);
          result = (MyaMirageDataResponseData)Engine.Engine.ProcessRequest(request, PurchaseEmailEngineRequests.MyaMirageData);
        }
        catch (Exception ex)
        {
          AtlantisException exception = new AtlantisException("PurchaseEmail.ShopperProductProvider.LoadMirageData", "0", ex.Message + ex.StackTrace, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }
      }

      return result;
    }

    private MyaRecentNamespacesResponseData LoadRecentNamespaces()
    {
      MyaRecentNamespacesResponseData result = null;

      if (!string.IsNullOrEmpty(_shopperContext.Value.ShopperId))
      {
        try
        {
          DateTime oneDayAgo = DateTime.Now.AddDays(-1);
          MyaRecentNamespacesRequestData request = new MyaRecentNamespacesRequestData(_shopperContext.Value.ShopperId, string.Empty, string.Empty, string.Empty, 0, oneDayAgo);
          result = (MyaRecentNamespacesResponseData)Engine.Engine.ProcessRequest(request, PurchaseEmailEngineRequests.MyaRecentNamespaces);
        }
        catch (Exception ex)
        {
          AtlantisException exception = new AtlantisException("PurchaseEmail.ShopperProductProvider.LoadRecentNamespaces", "0", ex.Message + ex.StackTrace, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }
      }

      return result;
    }

    private MyaMirageStatusResponseData LoadMirageStatus()
    {
      MyaMirageStatusResponseData result = null;

      if (!string.IsNullOrEmpty(_shopperContext.Value.ShopperId))
      {
        try
        {
          MyaMirageStatusRequestData request = new MyaMirageStatusRequestData(_shopperContext.Value.ShopperId, string.Empty, string.Empty, string.Empty, 0);
          result = (MyaMirageStatusResponseData)Engine.Engine.ProcessRequest(request, PurchaseEmailEngineRequests.MyaMirageStatus);
        }
        catch (Exception ex)
        {
          AtlantisException exception = new AtlantisException("PurchaseEmail.ShopperProductProvider.LoadMirageStatus", "0", ex.Message + ex.StackTrace, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }
      }

      return result;
    }

    public bool HasProductId(int productId)
    {
      bool result = false;
      if (_mirageData.Value != null)
      {
        result = _mirageData.Value.GetProductIdTotal(productId) > 0;
      }

      return result;
    }

    public bool HasProductNamespace(string productNamespace)
    {
      bool result = false;
      if (_mirageData.Value != null)
      {
        result = _mirageData.Value.GetProductNamespaceTotal(productNamespace) > 0;

        if (!result)
        {
          if ((_mirageStatus.Value != null) && (!_mirageStatus.Value.IsMirageCurrent))
          {
            if (_recentNamespaces.Value != null)
            {
              result = _recentNamespaces.Value.Namespaces.Contains(productNamespace);
            }
          }
        }
      }

      return result;
    }


  }
}
