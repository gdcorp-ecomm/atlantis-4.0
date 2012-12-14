using Atlantis.Framework.Interface;
using Atlantis.Framework.NewsCenter.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.NewsCenter.Impl
{
  public class ReleaseByDateRequest : IRequest
  {
    const string cacheDataFormat = "<getNewsReleaseByDate><param name='itemDate' value='{0}'/></getNewsReleaseByDate>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      ReleaseByDateRequestData releaseRequestData = (ReleaseByDateRequestData)requestData;
      string requestXml = string.Format(cacheDataFormat, releaseRequestData.DisplayDateKey);
      string resultXml = DataCache.DataCache.GetCacheData(requestXml);

      try
      {
        List<NewsRelease> releaseList = new List<NewsRelease>(50);

        XElement releaseData = XElement.Parse(resultXml);
        var releases = releaseData.Descendants("item");
        foreach (XElement releaseItem in releases)
        {
          NewsRelease release = NewsRelease.FromDataCacheXElement(releaseItem);
          if (release.IsValid)
          {
            releaseList.Add(release);
          }
        }

        result = new ReleaseByDateResponseData(releaseList);
       
      }
      catch(Exception ex)
      {
        result = new ReleaseByDateResponseData(requestData, ex);
      }

      return result;
    }
  }
}
