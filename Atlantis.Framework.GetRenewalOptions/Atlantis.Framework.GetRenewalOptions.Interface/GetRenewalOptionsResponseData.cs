using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetRenewalOptions.Interface
{
  [Serializable]
  public class GetRenewalOptionsResponseData : IResponseData
  {
    public string XML { get; set; }
    public string ToXML()
    {
      return XML;
    }

    public AtlantisException AtlException { get; set; }
    public AtlantisException GetException()
    {
      return AtlException;
    }
  }
}
