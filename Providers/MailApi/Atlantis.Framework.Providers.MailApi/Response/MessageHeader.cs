using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.MailApi.Response
{
  public class MessageHeader : IMessageHeader
  {
    public string Cc { get; set; }

    public string FileName { get; set; }

    public bool Flagged { get; set; }

    public double FolderNum { get; set; }

    public string From { get; set; }

    public string FromSort { get; set; }

    public bool HasAttachment { get; set; }

    public double HeaderNum { get; set; }

    public double InternalDate { get; set; }

    public bool IsAnswered { get; set; }

    public bool IsDraft { get; set; }

    public bool IsForwarded { get; set; }

    public bool IsSecure { get; set; }

    public string MessageId { get; set; }

    public string ModifiedDate { get; set; }

    public double MsgDate { get; set; }

    public double MsgSize { get; set; }

    public double MsgUid { get; set; }

    public bool IsPreferred { get; set; }

    public int Priority { get; set; }

    public bool IsRead { get; set; }

    public bool IsRecallable { get; set; }

    public string ReplyTo { get; set; }

    public string Subject { get; set; }

    public string SubjectSort { get; set; }

    public string To { get; set; }

    public string ToSort { get; set; }
  }
}
