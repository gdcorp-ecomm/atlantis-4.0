using System;
using System.Collections.Generic;
using Atlantis.Framework.BonsaiRenewalsByProductId.Interface.Types;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiRenewalsByProductId.Interface
{
  public class BonsaiProductRenewalResponseData : IResponseData
  {
    public AtlantisException AtlantisException { get; private set; }
    public int RenewalProductId { get; private set; }

    public BonsaiProductRenewalResponseData(int renewalProductId)
    {
      RenewalProductId = renewalProductId;
    }

    public BonsaiProductRenewalResponseData(AtlantisException atlEx)
    {
      AtlantisException = atlEx;
    }
    
    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in BonsaiProductRenewalResponseData"); 
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }
  }
}
