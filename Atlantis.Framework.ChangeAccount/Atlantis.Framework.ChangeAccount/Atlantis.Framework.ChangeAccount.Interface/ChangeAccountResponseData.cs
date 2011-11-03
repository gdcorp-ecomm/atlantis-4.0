using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ChangeAccount.Interface
{
  [Serializable]
  public class ChangeAccountResponseData : IResponseData
  {
    public ChangeAccountResponseData()
    {
      ChangeAccountResponses = new List<ChangeAccountResponseObject>();
    }

    public List<ChangeAccountResponseObject> ChangeAccountResponses { get; set; }

    public bool IsSuccess { get; set; }

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

    public int BasketResultCode { get; set; }
  }
}
