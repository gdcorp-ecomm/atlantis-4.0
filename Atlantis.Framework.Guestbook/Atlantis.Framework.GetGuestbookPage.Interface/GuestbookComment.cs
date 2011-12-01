using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.GetGuestbookPage.Interface
{
  public class GuestbookComment
  {
    private string _commentType = string.Empty;
    private int _commentId = 0;
    private int _commentStatusId = (int)GuestbookCommentStatus.PendingApproval;
    private DateTime _createDate = DateTime.Now;
    private int _guestbookId = 0;
    private string _comment = string.Empty;
    private string _guestName = string.Empty;
    private string _guestEmail = string.Empty;

    public GuestbookComment(int commentId, int guestbookId, string commentType, int commentStatusId,
      DateTime createDate, string comment, string guestName, string guestEmail)
    {
      _commentId = commentId;
      _guestbookId = guestbookId;
      _commentType = commentType;
      _commentStatusId = commentStatusId;
      _createDate = createDate;
      _comment = comment;
      _guestName = guestName;
      _guestEmail = guestEmail;
    }

    public string Type
    {
      get { return _commentType; }
    }

    public int Id
    {
      get { return _commentId; }
    }

    public int Status
    {
      get { return _commentStatusId; }
    }

    public DateTime CreateDate
    {
      get { return _createDate; }
    }

    public string Comment
    {
      get { return _comment; }
    }

    public string GuestName
    {
      get { return _guestName; }
    }

    public string GuestEmail
    {
      get { return _guestEmail; }
    }

    public int GuestbookId
    {
      get { return _guestbookId; }
    }
  }
}
