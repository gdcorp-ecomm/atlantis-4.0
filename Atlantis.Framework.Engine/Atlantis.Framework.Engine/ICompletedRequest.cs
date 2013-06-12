using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Engine
{
  public interface ICompletedRequest
  {
    ConfigElement Config { get; }
    RequestData RequestData { get; }
    IResponseData ResponseData { get; }
    TimeSpan ElapsedTime { get; }
    AtlantisException Exception { get; }
  }
}
