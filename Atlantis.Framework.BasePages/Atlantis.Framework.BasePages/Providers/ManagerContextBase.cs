using System;
using System.Collections.Specialized;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Shopper.Interface;

namespace Atlantis.Framework.BasePages.Providers
{
  public abstract class ManagerContextBase : ProviderBase, IManagerContext
  {
    private const int _WWD_PLID = 1387;

    protected ManagerContextBase(IProviderContainer container)
      : base(container)
    {
      ClearManagerContext();

      try
      {
        DetermineManager();
      }
      catch(Exception ex)
      {
        ClearManagerContext();
        string source = GetType().Name + ".DetermineManager";
        LogManagerException(source, ex.Message + ex.StackTrace, string.Empty);
      }
    }

    private void ClearManagerContext()
    {
      IsManager = false;
      ManagerContextId = ContextIds.Unknown;
      ManagerPrivateLabelId = 0;
      ManagerQuery = new NameValueCollection();
      ManagerShopperId = string.Empty;
      ManagerUserId = string.Empty;
      ManagerUserName = string.Empty;
    }

    protected abstract void DetermineManager();

    protected void LogManagerException(string sourceFunction, string message, string data)
    {
      var managerException = new AtlantisException(sourceFunction, string.Empty, "403", message, data, "none", string.Empty, string.Empty, string.Empty, 0);
      Engine.Engine.LogAtlantisException(managerException);
    }

    public bool IsManager { get; protected set; }
    public int ManagerContextId { get; protected set; }
    public int ManagerPrivateLabelId { get; protected set; }
    public NameValueCollection ManagerQuery { get; private set; }
    public string ManagerShopperId { get; protected set; }
    public string ManagerUserId { get; protected set; }
    public string ManagerUserName { get; protected set; }

    protected void SetManagerContext()
    {
      ManagerContextId = ContextIds.Unknown;
      if (!string.IsNullOrEmpty(ManagerShopperId))
      {
        int privateLabelId;
        if (VerifyShopper(ManagerShopperId, out privateLabelId))
        {
          ManagerPrivateLabelId = privateLabelId;
          IsManager = true;
          if (ManagerPrivateLabelId == 1)
          {
            ManagerContextId = ContextIds.GoDaddy;
          }
          else if (ManagerPrivateLabelId == 2)
          {
            ManagerContextId = ContextIds.BlueRazor;
          }
          else if (ManagerPrivateLabelId == _WWD_PLID)
          {
            ManagerContextId = ContextIds.WildWestDomains;
          }
          else
          {
            ManagerContextId = ContextIds.Reseller;
          }
        }
      }

    }

    private bool VerifyShopper(string shopperId, out int privateLabelId)
    {
      bool result = false;
      privateLabelId = 0;

      try
      {
        var request = new VerifyShopperRequestData(shopperId);
        var response = (VerifyShopperResponseData)Engine.Engine.ProcessRequest(request, BasePageEngineRequests.VerifyShopper);

        if (response.IsVerified)
        {
          privateLabelId = response.PrivateLabelId;
          result = true;
        }
        else
        {
          LogManagerException("ManagerContextBase.VerifyShopper", "Verify Shopper Failed.", "shopperid=" + shopperId);
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + ex.StackTrace;
        LogManagerException("ManagerContextBase.VerifyShopper", message, "shopperid=" + shopperId);
      }

      return result;
    }


  }
}
