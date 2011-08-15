using System;
using System.Collections.Generic;
using Atlantis.Framework.BonsaiRenewalsByProductId.Interface.Types;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiRenewalsByProductId.Interface
{
  public class BonsaiRenewalsResponseData : IResponseData
  {
    public AtlantisException AtlantisException { get; private set; }
    public List<BonsaiRenewalOption> RenewalOptions { get; private set; }

    public BonsaiRenewalsResponseData(List<BonsaiRenewalOption> renewalOptions)
    {
      RenewalOptions = renewalOptions;
    }

    public BonsaiRenewalsResponseData(AtlantisException atlEx)
    {
      AtlantisException = atlEx;
    }
    
    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in BonsaiRenewalsResponseData"); 
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }
  }
}
