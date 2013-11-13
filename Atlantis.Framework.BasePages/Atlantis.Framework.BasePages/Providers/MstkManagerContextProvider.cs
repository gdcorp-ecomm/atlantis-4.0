using Atlantis.Framework.Interface;
using Atlantis.Framework.MiniEncrypt;
using System;
using System.Web;

namespace Atlantis.Framework.BasePages.Providers
{
  public class MstkManagerContextProvider : ManagerContextBase
  {
    private const string _MSTK_KEY = "mstk";
    private const string _SHOPPER_KEY = "shopper_id";

    public MstkManagerContextProvider(IProviderContainer providerContainer)
      : base(providerContainer)
    {
    }

    private bool LookupManagerUser(string mstk, out string managerUserId, out string managerLoginName)
    {
      bool result = false;
      managerUserId = string.Empty;
      managerLoginName = string.Empty;

      try
      {
        if (!string.IsNullOrEmpty(mstk))
        {
          int status = -1;
          using (var mstkAuth = MstkAuthentication.CreateDisposable())
          {
            status = mstkAuth.ParseMstk(mstk, out managerUserId, out managerLoginName);
          }

          if (status == 0)
          {
            result = true;
          }
        }
      }
      catch (Exception ex)
      {
        LogManagerException("MstkManagerContextProvider.LookupManagerUser", ex.Message + ex.StackTrace, string.Empty);
      }

      return result;
    }

    protected override void DetermineManager()
    {
      if (RequestHelper.IsRequestInternal())
      {
        string mstk = HttpContext.Current.Request.QueryString[_MSTK_KEY];
        string shopperId = HttpContext.Current.Request.QueryString[_SHOPPER_KEY];
        if (!string.IsNullOrEmpty(mstk) && !string.IsNullOrEmpty(shopperId))
        {
          string managerUserId;
          string managerUserName;
          if (LookupManagerUser(mstk, out managerUserId, out managerUserName))
          {
            ManagerUserId = managerUserId;
            ManagerUserName = managerUserName;
            ManagerShopperId = shopperId;
            ManagerQuery[_SHOPPER_KEY] = shopperId;
            ManagerQuery[_MSTK_KEY] = mstk;
            SetManagerContext();
          }
        }
      }
    }
  }
}