using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Manager.Interface
{
  public class ManagerUserLookupRequestData : RequestData
  {
    private readonly string _userId;
    public string UserId
    {
      get { return _userId; }
    }

    private readonly int _managerCategoriesRequestType;
    public int ManagerCategoriesRequestType
    {
      get { return _managerCategoriesRequestType; }
    }

    public ManagerUserLookupRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string userId, int managerCategoriesRequestType = 462)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _userId = userId;
      _managerCategoriesRequestType = managerCategoriesRequestType;
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(_userId);
    }
  }
}
