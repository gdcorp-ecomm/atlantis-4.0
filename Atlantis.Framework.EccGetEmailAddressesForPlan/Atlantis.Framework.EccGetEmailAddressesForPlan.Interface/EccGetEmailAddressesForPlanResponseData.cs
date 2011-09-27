using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EccGetEmailAddressesForPlan.Interface
{
  public class EccGetEmailAddressesForPlanResponseData : EccResponseDataBase<List<String>>
  {
    public List<String> Results
    {
      get
      {
        IOrderedEnumerable<string> temp = Response.Item.Results[0].OrderBy(x => x.Substring(0, 1));
        return temp.ToList();
      }
    }

    public EccGetEmailAddressesForPlanResponseData(string resultJson) : base(resultJson)
    {
    }


    public EccGetEmailAddressesForPlanResponseData(AtlantisException atlantisException) : base(atlantisException)
    {
    }

    public EccGetEmailAddressesForPlanResponseData(RequestData requestData, Exception exception) : base(requestData, exception)
    {
    }
  }
}
