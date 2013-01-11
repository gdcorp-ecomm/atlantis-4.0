using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BazaarVoiceAPI.Interface
{
  public class ReviewsAPIResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private Dictionary<string, Review> _reviewsList;
    
    public ReviewsAPIResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "ReviewsAPIResponseData.constructor", ex.Message + ex.StackTrace, requestData.ToXML());
    }

    public ReviewsAPIResponseData(IEnumerable<Review> reviews)
    {
      if (reviews != null)
      {
        _reviewsList = new Dictionary<string, Review>();
        foreach (var review in reviews)
        {
          if (review.Id != null) _reviewsList.Add(review.Id, review);
        }
      }
      else
      {
        _reviewsList = null;
      }
    }

    public Dictionary<string, Review> Reviews
    {
      get { return _reviewsList; }
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
