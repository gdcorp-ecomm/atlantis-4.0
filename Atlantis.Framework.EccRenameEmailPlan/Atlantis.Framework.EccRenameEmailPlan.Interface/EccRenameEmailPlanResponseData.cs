using System;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EccRenameEmailPlan.Interface
{
  public class EccRenameEmailPlanResponseData : EccResponseDataBase<object>
  {

    public EccRenameEmailPlanResponseData(string resultJson) : base (resultJson)
    {}

    public EccRenameEmailPlanResponseData(AtlantisException atlantisException) : base (atlantisException)
    {
    }

    public EccRenameEmailPlanResponseData(RequestData requestData, Exception exception) : base(requestData, exception)
    {
    }


  }
  
}
