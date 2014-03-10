
using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Providers.MailApi.Response
{
  [DataContract]
  public class MessageHeader : IMessageHeader
  {
    [DataMember]
    public string Cc { get; set; }

    [DataMember]
    public string FileName { get; set; }

    [DataMember]
    public bool Flagged { get; set; }

    [DataMember]
    public double FolderNum { get; set; }

    [DataMember]
    public string From { get; set; }

    [DataMember]
    public string FromSort { get; set; }

    [DataMember]
    public bool HasAttachment { get; set; }

    [DataMember]
    public double HeaderNum { get; set; }

    [DataMember]
    public double InternalDate { get; set; }

    [DataMember]
    public bool IsAnswered { get; set; }

    [DataMember]
    public bool IsDraft { get; set; }

    [DataMember]
    public bool IsForwarded { get; set; }

    [DataMember]
    public bool IsSecure { get; set; }

    [DataMember]
    public string MessageId { get; set; }

    [DataMember]
    public string ModifiedDate { get; set; }

    [DataMember]
    public double MsgDate { get; set; }

    [DataMember]
    public double MsgSize { get; set; }

    [DataMember]
    public double MsgUid { get; set; }

    [DataMember]
    public bool IsPreferred { get; set; }

    [DataMember]
    public int Priority { get; set; }

    [DataMember]
    public bool IsRead { get; set; }

    [DataMember]
    public bool IsRecallable { get; set; }

    [DataMember]
    public string ReplyTo { get; set; }

    [DataMember]
    public string Subject { get; set; }

    [DataMember]
    public string SubjectSort { get; set; }

    [DataMember]
    public string To { get; set; }

    [DataMember]
    public string ToSort { get; set; }
  }
}
