using Atlantis.Framework.Interface;
using Atlantis.Framework.NewsCenter.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.NewsCenter.Impl
{
  public class ReleaseByDateAndTypeRequest : IRequest
  {
    const string cacheDataFormat = "<getNewsReleaseByDateAndType><param name='itemDate' value='{0}'/><param name='news_type_id' value='{1}'/></getNewsReleaseByDateAndType>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      ReleaseByDateAndTypeRequestData releaseRequestData = (ReleaseByDateAndTypeRequestData)requestData;
      string requestXml = string.Format(cacheDataFormat, releaseRequestData.DisplayDateKey, releaseRequestData.ReleaseType);
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

        result = new ReleaseByDateAndTypeResponseData(releaseList);

      }
      catch (Exception ex)
      {
        result = new ReleaseByDateAndTypeResponseData(requestData, ex);
      }

      return result;
    }
  }
}
