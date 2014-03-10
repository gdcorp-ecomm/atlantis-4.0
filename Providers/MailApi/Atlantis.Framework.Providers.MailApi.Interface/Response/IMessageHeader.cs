using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface IMessageHeader
  {
    string Cc { get; set; }

    string FileName { get; set; }

    bool Flagged { get; set; }

    double FolderNum { get; set; }

    string From { get; set; }

    string FromSort { get; set; }

    bool HasAttachment { get; set; }

    double HeaderNum { get; set; }

    double InternalDate { get; set; }

    bool IsAnswered { get; set; }

    bool IsDraft { get; set; }

    bool IsForwarded { get; set; }

    bool IsSecure { get; set; }

    string MessageId { get; set; }

    string ModifiedDate { get; set; }

    double MsgDate { get; set; }

    double MsgSize { get; set; }

    double MsgUid { get; set; }

    bool IsPreferred { get; set; }

    int Priority { get; set; }

    bool IsRead { get; set; }

    bool IsRecallable { get; set; }

    string ReplyTo { get; set; }

    string Subject { get; set; }

    string SubjectSort { get; set; }

    string To { get; set; }

    string ToSort { get; set; }
  }
}
