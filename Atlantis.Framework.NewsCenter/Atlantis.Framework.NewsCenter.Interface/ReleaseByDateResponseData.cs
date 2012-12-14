using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.NewsCenter.Interface
{
  public class ReleaseByDateResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private List<NewsRelease> _newsReleases;
    private Dictionary<string, NewsRelease> _newsReleasesById;
    private Dictionary<string, NewsRelease> _newsReleasesByUrlPath;

    public ReleaseByDateResponseData(List<NewsRelease> newsReleases)
    {
      _newsReleases = newsReleases;
      _newsReleasesById = new Dictionary<string, NewsRelease>(StringComparer.OrdinalIgnoreCase);
      _newsReleasesByUrlPath = new Dictionary<string, NewsRelease>(StringComparer.OrdinalIgnoreCase);

      if (newsReleases != null)
      {
        foreach (var newsRelease in _newsReleases)
        {
          _newsReleasesById[newsRelease.Id] = newsRelease;
          _newsReleasesByUrlPath[newsRelease.UrlPath] = newsRelease;
        }
      }
    }

    public IEnumerable<NewsRelease> NewsReleases
    {
      get { return _newsReleases; }
    }

    public NewsRelease GetReleaseByUrlPath(string urlPath)
    {
      NewsRelease result;
      if (!_newsReleasesByUrlPath.TryGetValue(urlPath, out result))
      {
        result = null;
      }
      return result;
    }

    public NewsRelease GetReleaseById(string id)
    {
      NewsRelease result;
      if (!_newsReleasesById.TryGetValue(id, out result))
      {
        result = null;
      }
      return result;
    }

    public ReleaseByDateResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "ReleaseByDateResponseData.constructor", ex.Message + ex.StackTrace, requestData.ToXML());
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
