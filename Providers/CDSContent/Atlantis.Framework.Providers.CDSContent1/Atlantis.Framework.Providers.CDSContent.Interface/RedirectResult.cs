using Atlantis.Framework.Providers.CDSContent.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantis.Framework.Providers.CDSContent.Interface
{
  public class RedirectResult : IRedirectResult
  {
    bool _redirectResult;
    IRedirectData _redirectData;

    public RedirectResult() { }

    public RedirectResult(bool redirectResult, IRedirectData redirectData)
    {
      _redirectResult = redirectResult;
      _redirectData = redirectData;
    }

    #region IRedirectResult Members

    public bool RedirectRequired
    {
      get { return _redirectResult; }
    }

    public IRedirectData RedirectData
    {
      get { return _redirectData; }
    }

    #endregion
  }
}
