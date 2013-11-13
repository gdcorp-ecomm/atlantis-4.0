﻿using Atlantis.Framework.Interface;
using Atlantis.Framework.MiniEncrypt;
using Atlantis.Framework.Shopper.Interface;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Atlantis.Framework.BasePages.Providers
{
  public class MstkManagerContextProvider : ProviderBase, IManagerContext
  {
    private const int _WWD_PLID = 1387;
    private const string _MSTK_KEY = "mstk";
    private const string _SHOPPER_KEY = "shopper_id";
    private readonly NameValueCollection _managerQuery = new NameValueCollection();
    private string _managerUserId = string.Empty;
    private string _managerUserName = string.Empty;
    private string _managerShopperId = string.Empty;
    private bool _isManager;
    private int _managerPrivateLabelId;
    private int _managerContextId = ContextIds.Unknown;

    public MstkManagerContextProvider(IProviderContainer providerContainer)
      : base(providerContainer)
    {
      DetermineManager();
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
        var aEx = new AtlantisException("MstkManagerContextProvider.LookupManagerUser", 0, ex.Message + ex.StackTrace, string.Empty);
        Engine.Engine.LogAtlantisException(aEx);
      }

      return result;
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
      }
      catch (Exception ex)
      {
        var aEx = new AtlantisException("MstkManagerContextProvider.VerifyShopper", 0, ex.Message + ex.StackTrace, string.Empty);
        Engine.Engine.LogAtlantisException(aEx);
      }

      return result;
    }

    private void SetManagerContext()
    {
      _managerContextId = ContextIds.Unknown;
      if (!string.IsNullOrEmpty(_managerShopperId))
      {
        if (VerifyShopper(_managerShopperId, out _managerPrivateLabelId))
        {
          _isManager = true;
          if (_managerPrivateLabelId == 1)
          {
            _managerContextId = ContextIds.GoDaddy;
          }
          else if (_managerPrivateLabelId == 2)
          {
            _managerContextId = ContextIds.BlueRazor;
          }
          else if (_managerPrivateLabelId == _WWD_PLID)
          {
            _managerContextId = ContextIds.WildWestDomains;
          }
          else
          {
            _managerContextId = ContextIds.Reseller;
          }
        }
      }
    }

    private void DetermineManager()
    {
      _isManager = false;

      try
      {
        if (RequestHelper.IsRequestInternal())
        {
          string mstk = HttpContext.Current.Request.QueryString[_MSTK_KEY];
          string shopperId = HttpContext.Current.Request.QueryString[_SHOPPER_KEY];
          if (!string.IsNullOrEmpty(mstk) && !string.IsNullOrEmpty(shopperId))
          {
            if (LookupManagerUser(mstk, out _managerUserId, out _managerUserName))
            {
              _managerShopperId = shopperId;
              _managerQuery[_SHOPPER_KEY] = shopperId;
              _managerQuery[_MSTK_KEY] = mstk;
              SetManagerContext();
            }
          }
        }
      }
      catch (Exception ex)
      {
        _isManager = false;
        _managerUserId = string.Empty;
        _managerUserName = string.Empty;
        _managerQuery.Clear();
        _managerContextId = 0;
        LogManagerException(ex.Message, ex.StackTrace);
      }
    }

    private void LogManagerException(string message, string data)
    {
      var managerException = new AtlantisException("MstkManagerContextProvider.DetermineManager", 403, message, data);
      Engine.Engine.LogAtlantisException(managerException);
    }

    #region IManagerContext Members

    public bool IsManager
    {
      get { return _isManager; }
    }

    public string ManagerUserId
    {
      get { return _managerUserId; }
    }

    public string ManagerUserName
    {
      get { return _managerUserName; }
    }

    public NameValueCollection ManagerQuery
    {
      get { return _managerQuery; }
    }

    public string ManagerShopperId
    {
      get { return _managerShopperId; }
    }

    public int ManagerContextId
    {
      get { return _managerContextId; }
    }

    public int ManagerPrivateLabelId
    {
      get { return _managerPrivateLabelId; }
    }

    #endregion
  }
}