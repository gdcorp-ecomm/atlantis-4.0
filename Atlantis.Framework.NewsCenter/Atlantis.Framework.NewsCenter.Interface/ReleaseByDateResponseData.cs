using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.NewsCenter.Interface
{
  public class ReleaseByDateResponseData : ReleaseResponseDataBase
  {
    public ReleaseByDateResponseData(RequestData requestData, Exception ex)
      : base(requestData, ex)
    { }

    public ReleaseByDateResponseData(List<NewsRelease> newsReleases)
      : base(newsReleases)
    { }

  }
}
