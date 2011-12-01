using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using System.Security.Cryptography;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.GetGuestbookPage.Interface
{
  public class GetGuestbookPageRequestData : RequestData
  {
    private int _guestbookId = 0;
    private string _domain = string.Empty;
    private int _commentsPerPage = 25;
    private int _firstCommentRow = 1;
    private int _commentStatusId = (int)GuestbookCommentStatus.Approved;

    public int GuestbookId
    {
      get { return _guestbookId; }
      set { _guestbookId = value; }
    }

    public string Domain
    {
      get { return _domain; }
      set
      {
        if (value == null)
          throw new ArgumentException("Domain cannot be set to null.");
        _domain = value;
      }
    }

    public int CommentsPerPage
    {
      get { return _commentsPerPage; }
      set
      {
        if (value < 1)
          throw new ArgumentException("Comments per page cannot be < 1.");

        _commentsPerPage = value;
      }
    }

    public int FirstCommentRow
    {
      get { return _firstCommentRow; }
      set
      {
        if (value < 1)
          throw new ArgumentException("First comment row cannot be less than 1.");

        _firstCommentRow = value;
      }
    }

    public int CommentStatusId
    {
      get { return _commentStatusId; }
      set { _commentStatusId = value; }
    }

    public GetGuestbookPageRequestData(
      string sShopperID, string sSourceURL, string sOrderID, string sPathway, int iPageCount, int guestbookId, string domain)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      GuestbookId = guestbookId;
      Domain = domain;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(ToXML());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      return BitConverter.ToString(md5Bytes, 0).Replace("-", "");
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("GetGuestbookPage");
      xtwRequest.WriteAttributeString("Domain", _domain);
      xtwRequest.WriteAttributeString("GuestbookId", _guestbookId.ToString());
      xtwRequest.WriteAttributeString("FirstRow", _firstCommentRow.ToString());
      xtwRequest.WriteAttributeString("PerPage", _commentsPerPage.ToString());
      xtwRequest.WriteAttributeString("Status", _commentStatusId.ToString());
      xtwRequest.WriteEndElement();

      return sbRequest.ToString();
    }

  }
}
