using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.NewsCenter.Interface
{
  public class ReleaseByDateAndTypeResponseData : ReleaseResponseDataBase
  {
    public ReleaseByDateAndTypeResponseData(RequestData requestData, Exception ex)
      : base(requestData, ex)
    { }

    public ReleaseByDateAndTypeResponseData(List<NewsRelease> newsReleases)
      : base(newsReleases)
    { }
  }
}
