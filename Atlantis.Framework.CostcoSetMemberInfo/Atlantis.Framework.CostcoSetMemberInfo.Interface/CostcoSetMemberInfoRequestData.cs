using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CostcoSetMemberInfo.Interface
{
  public class CostcoSetMemberInfoRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(20);

    public CostcoSetMemberInfoRequestData(string shopperId,
                                             string sourceUrl,
                                             string orderId,
                                             string pathway,
                                             int pageCount,
                                             string costcoMembershipId,
                                             string shopperPostalCode,
                                             int resellerId)
      : this(shopperId, sourceUrl, orderId, pathway, pageCount, costcoMembershipId, shopperPostalCode, resellerId, _defaultRequestTimeout)
    {
    }

    public CostcoSetMemberInfoRequestData(string shopperId,
                                             string sourceUrl,
                                             string orderId,
                                             string pathway,
                                             int pageCount,
                                             string costcoMembershipId,
                                             string shopperPostalCode,
                                             int resellerId,
                                             TimeSpan requestTimeout)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      CostcoMembershipId = costcoMembershipId;
      ShopperPostalCode = shopperPostalCode;
      ResellerId = resellerId;
      RequestTimeout = requestTimeout;
    }

    public static bool HasValidCostcoMembershipIdFormat(string costcoMembershipId)
    {
      if (costcoMembershipId.Length != 12)
      {
        return false;
      }
      foreach (char c in costcoMembershipId)
      {
        if (!char.IsDigit(c))
        {
          return false;
        }
      }
      return true;
    }

    public string ShopperPostalCode { get; set; }
    private string _CostcoMembershipId;
    public string CostcoMembershipId 
    { 
      get { return _CostcoMembershipId; } 
      set 
      { 
        if ( !HasValidCostcoMembershipIdFormat(value) )
        {
          throw new FormatException("Invalid Costco membership ID.");
        }
        _CostcoMembershipId = value;
      }
    }
    public int ResellerId { get; set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("This data is not cacheable");
    }


  }
}
