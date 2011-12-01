using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AddGuestbookComment.Interface;
using Atlantis.Framework.AddGuestbookComment.Impl.GuestbookAddWS;

namespace Atlantis.Framework.AddGuestbookComment.Impl
{
  public class AddGuestbookCommentRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      AddGuestbookCommentResponseData responseData = null;

      try
      {
        AddGuestbookCommentRequestData guestbookData = (AddGuestbookCommentRequestData)oRequestData;
        GuestbookService guestbookService = new GuestbookService();
        guestbookService.Url = ((WsConfigElement)oConfig).WSURL;

        CommentStatusEnum status = CommentStatusEnum.Pending;
        switch (guestbookData.Status)
        {
          case GuestbookCommentStatus.Approved:
            status = CommentStatusEnum.Approved;
            break;
        }

        WsComment comment = new WsComment();
        comment.CommentStatusId = status;
        comment.CreateDate = DateTime.Now;
        comment.GuestbookId = guestbookData.GuestbookId;
        comment.GuestComment = guestbookData.Comment;
        comment.GuestEmail = guestbookData.GuestEmail;
        comment.GuestName = guestbookData.GuestName;

        GuestbookResultEnum result = guestbookService.AddGuestbookEntry(
          guestbookData.GuestbookId, guestbookData.Domain, comment, guestbookData.CommentType);

        if (result != GuestbookResultEnum.Success)
        {
          AtlantisException ex = new AtlantisException(
            oRequestData, "AddGuestbookCommentResponseData", result.ToString(), oRequestData.ToXML());
          responseData = new AddGuestbookCommentResponseData(ex);
        }
        else
        {
          responseData = new AddGuestbookCommentResponseData(result.ToString());
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new AddGuestbookCommentResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        AtlantisException aEx = new AtlantisException(
          oRequestData, "AddGuestbookCommentResponseData", ex.Message, oRequestData.ToXML(), ex);
        responseData = new AddGuestbookCommentResponseData(aEx);
      }

      return responseData;
    }

    #endregion

  }
}
