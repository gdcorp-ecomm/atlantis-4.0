using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddGuestbookComment.Interface
{
  public class AddGuestbookCommentRequestData : RequestData
  {
    private int _guestbookId;
    private string _domain;
    private string _guestName = string.Empty;
    private string _guestEmail = string.Empty;
    private string _comment = string.Empty;
    private int _commentType = 0;
    private GuestbookCommentStatus _commentStatus = GuestbookCommentStatus.PendingApproval;

    public int GuestbookId
    {
      get { return _guestbookId; }
    }

    public string Domain
    {
      get { return _domain; }
    }

    public string GuestName
    {
      get { return _guestName; }
    }

    public string GuestEmail
    {
      get { return _guestEmail; }
    }

    public string Comment
    {
      get { return _comment; }
    }

    public int CommentType
    {
      get { return _commentType; }
      set { _commentType = value; }
    }

    public GuestbookCommentStatus Status
    {
      get { return _commentStatus; }
      set { _commentStatus = value; }
    }

    public AddGuestbookCommentRequestData(
      string sShopperID, string sSourceURL, string sOrderID, string sPathway, int iPageCount, 
      int guestbookId, string domain, string guestName, string guestEmail, string comment)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      _guestbookId = guestbookId;
      _domain = domain;
      _guestEmail = guestEmail;
      _guestName = guestName;
      _comment = comment;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("AddGuestbookComment is not cacheable.");
    }

  }
}
