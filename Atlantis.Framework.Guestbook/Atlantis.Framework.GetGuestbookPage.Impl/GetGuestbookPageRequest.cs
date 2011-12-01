using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.GetGuestbookPage.Interface;
using Atlantis.Framework.GetGuestbookPage.Impl.GuestBookWebService;

namespace Atlantis.Framework.GetGuestbookPage.Impl
{
  public class GetGuestbookPageRequest : IRequest
  {

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      GetGuestbookPageResponseData responseData = null;

      try
      {
        GetGuestbookPageRequestData guestbookData = (GetGuestbookPageRequestData)oRequestData;
        GuestbookService guestbookService = new GuestbookService();
        guestbookService.Url = ((WsConfigElement)oConfig).WSURL;

        int totalPages = 0;
        int totalEntries = 0;
        WsComment[] commentList;

        GuestbookResultEnum result = guestbookService.GetGuestbookPage(
          guestbookData.Domain, guestbookData.GuestbookId, guestbookData.CommentStatusId,
          guestbookData.FirstCommentRow, guestbookData.CommentsPerPage, out totalPages,
          out totalEntries, out commentList);

        if (result != GuestbookResultEnum.Success)
        {
          AtlantisException ex = new AtlantisException(
            oRequestData, "GetGuestbookPageRequest", result.ToString(), oRequestData.ToXML());
          responseData = new GetGuestbookPageResponseData(ex);
        }
        else
        {
          IEnumerable<GuestbookComment> comments = ConvertToGuestbookComments(commentList);
          responseData = new GetGuestbookPageResponseData(totalPages, totalEntries, comments);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new GetGuestbookPageResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        AtlantisException aEx = new AtlantisException(
          oRequestData, "GetGuestbookPageRequest", ex.Message, oRequestData.ToXML(), ex);
        responseData = new GetGuestbookPageResponseData(aEx);
      }

      return responseData;
    }

    #endregion

    private IEnumerable<GuestbookComment> ConvertToGuestbookComments(WsComment[] commentList)
    {
      List<GuestbookComment> result = new List<GuestbookComment>(commentList.Length);
      foreach (WsComment wsComment in commentList)
      {
        string commentType = string.Empty;
        string commentText = wsComment.GuestComment;
        if ((commentText.Length > 1) && (commentText[1] == '~'))
        {
          commentType = commentText[0].ToString();
          if (commentText.Length > 2)
          {
            commentText = commentText.Substring(2);
          }
          else
          {
            commentText = string.Empty;
          }
        }

        GuestbookComment newComment = new GuestbookComment(
          wsComment.CommentId, wsComment.GuestbookId, commentType, (int)wsComment.CommentStatusId,
          wsComment.CreateDate, commentText, wsComment.GuestName, wsComment.GuestEmail);
        result.Add(newComment);
      }
      return result;
    }

  }
}
