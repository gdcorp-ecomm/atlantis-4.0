using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.NewsCenter.Interface
{
  public abstract class ReleaseResponseDataBase : IResponseData
  {
    private AtlantisException _exception = null;
    private List<NewsRelease> _newsReleases;
    private Dictionary<string, NewsRelease> _newsReleasesById;
    private Dictionary<string, NewsRelease> _newsReleasesByUrlPath;

    public ReleaseResponseDataBase(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "ReleaseResponseDataBase.constructor", ex.Message + ex.StackTrace, requestData.ToXML());
    }

    public ReleaseResponseDataBase(List<NewsRelease> newsReleases)
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

    public List<NewsRelease> FindAll(Predicate<NewsRelease> match)
    {
      return _newsReleases.FindAll(match);
    }

    public NewsRelease Find(Predicate<NewsRelease> match)
    {
      return _newsReleases.Find(match);
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
