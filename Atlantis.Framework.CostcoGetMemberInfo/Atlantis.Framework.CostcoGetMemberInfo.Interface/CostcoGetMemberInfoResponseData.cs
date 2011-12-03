using System;
using Atlantis.Framework.Interface;
namespace Atlantis.Framework.CostcoGetMemberInfo.Interface
{
  public class CostcoGetMemberInfoResponseData : IResponseData
  {

    public int MemberLevelId { get; private set; }

    public CostcoGetMemberInfoResponseData(int memberLevelId)
    {
      MemberLevelId = memberLevelId;
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      // don't bother using this, just throw any exeception from the request
      return null;
    }

    #endregion
  }
}
