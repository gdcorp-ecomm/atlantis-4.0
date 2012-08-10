
namespace Atlantis.Framework.PromoOffering.Interface
{
  public class ResellerPromoItem
  {
    public ResellerPromoItem(string description, int groupId, bool isActive, int promoGroupId)
    {
      Description = description;
      GroupId = groupId;
      IsActive = isActive;
      PromoGroupId = promoGroupId;
    }

    public string Description
    {
      get;
      private set;
    }

    public int GroupId
    {
      get;
      private set;
    }

    public bool IsActive
    {
      get;
      private set;
    }

    public int PromoGroupId
    {
      get;
      private set;
    }
  }
}

