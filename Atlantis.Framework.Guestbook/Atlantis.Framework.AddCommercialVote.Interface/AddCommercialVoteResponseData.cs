using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddCommercialVote.Interface
{
  public class AddCommercialVoteResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private int _result;

    public AddCommercialVoteResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public AddCommercialVoteResponseData(int result)
    {
      _result = result;
    }

    public int Result
    {
      get { return _result; }
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
