using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CostcoSetMemberInfo.Interface
{
  public class CostcoSetMemberInfoResponseData : IResponseData
  {
    #region Constructors

    public CostcoSetMemberInfoResponseData(string response, bool bIsSuccess, string msgForUser, int ? memberLevel, bool ? discountDomainClub, bool ? existingMember)
    {
      IsSuccess = bIsSuccess;
      Response = response;
      MemberLevel = memberLevel;
      DiscountDomainClub = discountDomainClub;
      ExistingMember = existingMember;
      MessageForUser = !bIsSuccess && String.IsNullOrEmpty(msgForUser) ? kMessageToUserOnException : msgForUser;

    }

    #endregion 

    #region Helpers

    private static readonly string kMessageToUserOnException = "An error occurred while assigning your Costco membership ID. If it persists, please contact Customer Support.";

    #endregion

    public bool IsSuccess
    {
      get;
      private set;
    }

    public int ? MemberLevel
    {
      get;
      private set;
    }

    public bool ? DiscountDomainClub
    {
      get;
      private set;
    }

    public bool ? ExistingMember
    {
      get;
      private set;
    }

    public string Response
    {
      get;
      private set;
    }

    #region IResponseData Members

    public string ToXML()
    {
      return Response;
    }

    public AtlantisException GetException()
    {
      return null;
    }

    public string MessageForUser
    {
      get;
      private set;
    }

    #endregion
  }
}
