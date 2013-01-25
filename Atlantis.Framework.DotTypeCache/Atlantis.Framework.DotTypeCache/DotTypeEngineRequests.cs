﻿namespace Atlantis.Framework.DotTypeCache
{
  public static class DotTypeEngineRequests
  {
    private static int _registryRequest = 639;
    private static int _productIdListRequest = 640;
    private static int _activeTldsRequest = 635;
    private static int _offeredTldsRequest = 637;
    private static int _tldmlByNameRequest = 634;

    public static int TLDMLByName
    {
      get { return _tldmlByNameRequest; }
      set { _tldmlByNameRequest = value; }
    }

    public static int Registry
    {
      get { return _registryRequest; }
      set { _registryRequest = value; }
    }

    public static int ProductIdList
    {
      get { return _productIdListRequest; }
      set { _productIdListRequest = value; }
    }

    public static int ActiveTlds
    {
      get { return _activeTldsRequest; }
      set { _activeTldsRequest = value; }
    }

    public static int OfferdTlds
    {
      get { return _offeredTldsRequest; }
      set { _offeredTldsRequest = value; }
    }
  }
}