namespace Atlantis.Framework.DotTypeCache
{
  public static class DotTypeEngineRequests
  {
    private static int _registryRequest = 639;
    private static int _productIdListRequest = 640;
    private static int _activeTldsRequest = 635;
    private static int _offeredTldsRequest = 637;
    private static int _tldmlByNameRequest = 634;
    private static int _domainContactFieldsRequest = 651;
    private static int _languagesRequest = 655;
    private static int _validDotTypesRequest = 667;
    private static int _tldPhaseDateListRequest = 744;
    private static int _productDomainAttributesRequest = 745;
    private static int _privateLabelTypeRequest = 662;
    private static int _tldAvailabilityRequest = 753;

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

    public static int DomainContactFields
    {
      get { return _domainContactFieldsRequest; }
      set { _domainContactFieldsRequest = value; }
    }

    public static int Languages
    {
      get { return _languagesRequest; }
      set { _languagesRequest = value; }
    }

    public static int ValidDotTypes
    {
      get { return _validDotTypesRequest; }
      set { _validDotTypesRequest = value; }
    }

    public static int ProductDomainAttributes
    {
      get { return _productDomainAttributesRequest; }
      set { _productDomainAttributesRequest = value; }
    }

    public static int PrivateLabelType
    {
      get { return _privateLabelTypeRequest; }
      set { _privateLabelTypeRequest = value; }
    }

    public static int TldPhaseDateList
    {
      get { return _tldPhaseDateListRequest; }
      set { _tldPhaseDateListRequest = value; }
    }

    public static int TldAvailabilityRequest
    {
      get { return _tldAvailabilityRequest; }
      set { _tldAvailabilityRequest = value; }
    }
  }
}
