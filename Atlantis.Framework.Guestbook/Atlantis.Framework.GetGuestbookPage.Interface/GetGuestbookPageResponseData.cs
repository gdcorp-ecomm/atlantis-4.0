using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetGuestbookPage.Interface
{
  public class GetGuestbookPageResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private int _totalPages = 0;
    private int _totalComments = 0;
    List<GuestbookComment> _comments = new List<GuestbookComment>();
    
    public GetGuestbookPageResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public GetGuestbookPageResponseData(int totalPages, int totalComments, IEnumerable<GuestbookComment> comments)
    {
      _totalPages = totalPages;
      _totalComments = totalComments;
      _comments = new List<GuestbookComment>(comments);
    }

    public int TotalPages
    {
      get { return _totalPages; }
    }

    public int TotalComments
    {
      get { return _totalComments; }
    }

    public List<GuestbookComment> GetComments()
    {
      List<GuestbookComment> result = new List<GuestbookComment>(_comments);
      return result;
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
